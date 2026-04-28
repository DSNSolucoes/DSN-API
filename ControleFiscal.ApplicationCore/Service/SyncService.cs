using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Domain.Interface.Service;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text.Json;

namespace ControleFiscal.ApplicationCore.Service
{
    public class SyncService : ISyncService
    {
        private readonly ContextLocalContext _db;
        private readonly IHttpClientFactory _httpFactory;
        private readonly ILogger<SyncService> _logger;

        private static readonly JsonSerializerOptions _jsonOpts =
            new() { PropertyNameCaseInsensitive = true };

        public SyncService(ContextLocalContext db, IHttpClientFactory httpFactory, ILogger<SyncService> logger)
        {
            _db = db;
            _httpFactory = httpFactory;
            _logger = logger;
        }

        // ----------------------------------------------------------------- //
        //  ObterStatus
        // ----------------------------------------------------------------- //
        public async Task<SyncStatusDto> ObterStatusAsync()
        {
            var cfg = await _db.SyncConfigs.FirstOrDefaultAsync();
            var pendentes = await _db.SyncLogs.CountAsync(s => s.Status == "PENDING");
            bool online = false;
            if (!string.IsNullOrWhiteSpace(cfg?.UrlNuvem))
            {
                try
                {
                    var client = _httpFactory.CreateClient();
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var resp = await client.GetAsync(cfg.UrlNuvem.TrimEnd('/') + "/health");
                    online = resp.IsSuccessStatusCode;
                }
                catch { online = false; }
            }

            return new SyncStatusDto
            {
                EmSincronizacao = cfg?.EmSincronizacao ?? false,
                UltimoSync = cfg?.UltimoSync,
                UrlNuvem = cfg?.UrlNuvem,
                PendenteLocal = pendentes,
                ConectividadeNuvem = online
            };
        }

        // ----------------------------------------------------------------- //
        //  Configurar URL
        // ----------------------------------------------------------------- //
        public async Task ConfigurarUrlNuvemAsync(string url)
        {
            var cfg = await _db.SyncConfigs.FirstOrDefaultAsync()
                      ?? new SyncConfig { Id = Guid.NewGuid().ToString("D") };

            cfg.UrlNuvem = url;
            if (cfg.Id == null) _db.SyncConfigs.Add(cfg);
            else _db.SyncConfigs.Update(cfg);

            await _db.SaveChangesAsync();
        }

        // ----------------------------------------------------------------- //
        //  Logs
        // ----------------------------------------------------------------- //
        public IEnumerable<SyncLog> ObterLogs(int pagina = 0, int tamanho = 50)
            => _db.SyncLogs
                  .OrderByDescending(s => s.CreatedAt)
                  .Skip(pagina * tamanho)
                  .Take(tamanho)
                  .ToList();

        // ----------------------------------------------------------------- //
        //  Sincronizar
        // ----------------------------------------------------------------- //
        public async Task<SyncResultDto> SincronizarAsync(CancellationToken ct = default)
        {
            var result = new SyncResultDto();

            // Busca configuração
            var cfg = await _db.SyncConfigs.FirstOrDefaultAsync(cancellationToken: ct);
            if (cfg == null || string.IsNullOrWhiteSpace(cfg.UrlNuvem))
            {
                result.Sucesso = false;
                result.Mensagem = "URL da nuvem não configurada.";
                return result;
            }

            if (cfg.EmSincronizacao)
            {
                result.Sucesso = false;
                result.Mensagem = "Sincronização já está em andamento.";
                return result;
            }

            // Marca início
            cfg.EmSincronizacao = true;
            _db.SyncConfigs.Update(cfg);
            await _db.SaveChangesAsync(ct);

            try
            {
                var client = _httpFactory.CreateClient();
                client.BaseAddress = new Uri(cfg.UrlNuvem.TrimEnd('/') + "/");
                client.Timeout = TimeSpan.FromSeconds(30);

                // 1. ENVIAR registros PENDING locais
                var pendentes = await _db.SyncLogs
                    .Where(s => s.Status == "PENDING")
                    .OrderBy(s => s.CreatedAt)
                    .Take(500)
                    .ToListAsync(ct);

                if (pendentes.Any())
                {
                    var pacote = new SyncPacoteDto
                    {
                        OrigemId = cfg.Id,
                        EnviadoEm = DateTime.UtcNow,
                        Registros = pendentes.Select(p => new SyncRegistroDto
                        {
                            Tabela = p.Tabela,
                            RegistroId = p.RegistroId,
                            Operacao = p.Operacao,
                            Payload = p.Payload ?? "{}",
                            AtualizadoEm = p.CreatedAt
                        }).ToList()
                    };

                    var response = await client.PostAsJsonAsync("api/sync/receber", pacote, ct);
                    if (response.IsSuccessStatusCode)
                    {
                        foreach (var log in pendentes)
                        {
                            log.Status = "SYNCED";
                            log.SyncedAt = DateTime.UtcNow;
                        }
                        await _db.SaveChangesAsync(ct);
                        result.RegistrosEnviados = pendentes.Count;
                    }
                    else
                    {
                        var body = await response.Content.ReadAsStringAsync(ct);
                        result.Erros.Add($"Erro ao enviar para nuvem: {(int)response.StatusCode} - {body}");
                    }
                }

                // 2. RECEBER alterações da nuvem (delta desde ultimo sync)
                var desde = cfg.UltimoSync?.ToString("o") ?? "2000-01-01T00:00:00Z";
                var getResp = await client.GetAsync($"api/sync/alteracoes?desde={Uri.EscapeDataString(desde)}", ct);
                if (getResp.IsSuccessStatusCode)
                {
                    var alteracoes = await getResp.Content
                        .ReadFromJsonAsync<SyncAlteracoesNuvemDto>(_jsonOpts, ct);

                    if (alteracoes?.Registros != null)
                    {
                        foreach (var reg in alteracoes.Registros)
                        {
                            try { await AplicarAlteracaoLocalAsync(reg, ct); result.RegistrosRecebidos++; }
                            catch (Exception ex)
                            {
                                result.Conflitos++;
                                result.Erros.Add($"Conflito [{reg.Tabela}/{reg.RegistroId}]: {ex.Message}");
                                _logger.LogWarning(ex, "Conflito ao aplicar alteração da nuvem {Tabela}/{Id}", reg.Tabela, reg.RegistroId);
                            }
                        }
                        await _db.SaveChangesAsync(ct);
                    }
                }

                // 3. Atualiza UltimoSync
                cfg.UltimoSync = DateTime.UtcNow;
                cfg.EmSincronizacao = false;
                _db.SyncConfigs.Update(cfg);
                await _db.SaveChangesAsync(ct);

                result.Sucesso = true;
                result.Mensagem = "Sincronização concluída.";
                result.UltimoSync = cfg.UltimoSync;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante sincronização.");
                result.Sucesso = false;
                result.Mensagem = ex.Message;
                result.Erros.Add(ex.Message);

                // Garante que EmSincronizacao seja desligado mesmo em erro
                try
                {
                    cfg.EmSincronizacao = false;
                    _db.SyncConfigs.Update(cfg);
                    await _db.SaveChangesAsync(CancellationToken.None);
                }
                catch { /* nao bloquear */ }
            }

            return result;
        }

        // ----------------------------------------------------------------- //
        //  Aplica um registro recebido da nuvem no banco local (last-write-wins)
        // ----------------------------------------------------------------- //
        private async Task AplicarAlteracaoLocalAsync(SyncRegistroDto reg, CancellationToken ct)
        {
            // Estratégia simples: registrar o que a nuvem mandou no SYNC_LOG como recebido.
            // A aplicação real nas tabelas de domínio requer deserializar para cada tipo — 
            // deixamos registrado para auditoria. Em produção adicione handlers por tabela.
            var existente = await _db.SyncLogs
                .FirstOrDefaultAsync(s => s.RegistroId == reg.RegistroId
                                       && s.Tabela == reg.Tabela
                                       && s.Operacao == reg.Operacao, ct);

            if (existente == null)
            {
                _db.SyncLogs.Add(new SyncLog
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Tabela = reg.Tabela,
                    RegistroId = reg.RegistroId,
                    Operacao = reg.Operacao,
                    Status = "RECEIVED",
                    Payload = reg.Payload,
                    CreatedAt = reg.AtualizadoEm,
                    SyncedAt = DateTime.UtcNow
                });
            }
        }
    }
}
