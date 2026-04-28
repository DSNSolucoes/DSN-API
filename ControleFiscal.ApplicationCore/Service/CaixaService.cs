using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Services.Interfaces;
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Domain.Model;
using System.Text.Json;

namespace ControleFiscal.ApplicationCore.Service
{
    public class CaixaService : ICaixaService
    {
        private readonly ContextLocalContext _contextLocal;

        public CaixaService(ContextLocalContext contextLocal)
        {
            _contextLocal = contextLocal;
        }

        public List<CaixaDTO> Listar(string lojaId, DateTime data)
        {
            ValidarData(lojaId, data);

            var nomeLoja = _contextLocal.Lojas
                .Where(x => x.Id == lojaId)
                .Select(x => x.Nome)
                .FirstOrDefault();

            var tiposValor = _contextLocal.TipoValorCaixa
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.Descricao)
                .Select(t => new TipoValor { Id = t.Id, Descricao = t.Descricao })
                .ToList();

            var caixas = _contextLocal.Caixa
                .Where(c => c.LojaId == lojaId && c.Ativo == "V")
                .OrderBy(c => c.Descricao)
                .ToList();

            var caixaIds = caixas.Select(c => c.Id).ToList();
            var tipoIds  = tiposValor.Select(t => t.Id).ToList();

            var movimentacoes = _contextLocal.CaixaMovimentacao
                .ToList()
                .Where(m =>
                    caixaIds.Contains(m.CaixaId) &&
                    tipoIds.Contains(m.TipoValorCaixaId) &&
                    m.Ativo == "V" &&
                    m.DataCompetencia.HasValue &&
                    m.DataCompetencia.Value.Date == data.Date)
                .ToList();

            return caixas.Select(caixa =>
            {
                var valores = tiposValor.Select(tipo =>
                {
                    var movs = movimentacoes
                        .Where(m => m.CaixaId == caixa.Id && m.TipoValorCaixaId == tipo.Id)
                        .OrderBy(m => m.DataRealizacao ?? data)
                        .ToList();

                    var detalhes = movs.Any()
                        ? movs.Select(m => new CaixaMovimentacaoDetalhesDTO
                        {
                            Id = m.Id,
                            Descricao = m.Descricao ?? string.Empty,
                            Valor = m.Valor,
                            DataCadastro = m.DataCadastro ?? data,
                            DataRealizacao = m.DataRealizacao ?? data,
                            AnoCompetencia = data.Year,
                            MesCompetencia = data.Month,
                            NomeFuncionario = m.NomeFuncionario ?? string.Empty,
                            AnexoNome = m.AnexoNome,
                            AnexoContentType = m.AnexoContentType,
                            AnexoArquivo = m.AnexoArquivo
                        }).ToList()
                        : new List<CaixaMovimentacaoDetalhesDTO>
                        {
                            new() { Id = string.Empty, Descricao = string.Empty, Valor = 0, DataCadastro = data,
                                    DataRealizacao = data, AnoCompetencia = data.Year, MesCompetencia = data.Month,
                                    NomeFuncionario = string.Empty }
                        };

                    return new CaixaMovimentacoesDTO
                    {
                        Id = movs.FirstOrDefault()?.Id ?? string.Empty,
                        TipoValorCaixaId = tipo.Id,
                        TipoValorCaixa = tipo,
                        ValorTotal = movs.Sum(x => x.Valor),
                        Detalhes = detalhes
                    };
                }).ToList();

                return new CaixaDTO
                {
                    Id = caixa.Id,
                    LojaId = caixa.LojaId,
                    NomeLoja = nomeLoja,
                    Descricao = caixa.Descricao,
                    AnoCompetencia = (short)data.Year,
                    MesCompetencia = (short)data.Month,
                    Valores = valores
                };
            }).ToList();
        }

        public List<CaixaResumoMensalDTO> ListarResumoMensal(string lojaId, int ano, int mes)
        {
            if (string.IsNullOrWhiteSpace(lojaId)) throw new ArgumentException("LojaId e obrigatorio.");
            if (ano <= 0)         throw new ArgumentException("Ano invalido.");
            if (mes < 1 || mes > 12) throw new ArgumentException("Mes invalido.");

            var caixas = _contextLocal.Caixa
                .Where(c => c.LojaId == lojaId && c.Ativo == "V")
                .OrderBy(c => c.Descricao).ToList();

            var caixaIds = caixas.Select(c => c.Id).ToList();

            var movs = _contextLocal.CaixaMovimentacao.ToList()
                .Where(m => caixaIds.Contains(m.CaixaId) && m.Ativo == "V" &&
                            m.DataCompetencia.HasValue &&
                            m.DataCompetencia.Value.Year == ano &&
                            m.DataCompetencia.Value.Month == mes).ToList();

            return caixas.Select(c => new CaixaResumoMensalDTO
            {
                CaixaId = c.Id,
                DescricaoCaixa = c.Descricao,
                ValorTotalMes = movs.Where(m => m.CaixaId == c.Id).Sum(m => m.Valor)
            }).ToList();
        }

        public Caixa IncluirCaixa(CaixaSalvarModel model)
        {
            ValidarModelCaixa(model);
            ValidarLoja(model.LojaId);
            ValidarDuplicidadeCaixa(model, null);

            var entity = new Caixa
            {
                Id = Guid.NewGuid().ToString("D"),
                LojaId = model.LojaId,
                Descricao = model.Descricao!.Trim(),
                DataCadastro = DateTime.Now,
                Ativo = "V",
                CreatedAt = DateTime.UtcNow,
                SyncStatus = "PENDING"
            };

            _contextLocal.Caixa.Add(entity);
            RegistrarSyncLog("CAIXA", entity.Id, "INSERT", entity);
            _contextLocal.SaveChanges();
            return entity;
        }

        public Caixa AlterarCaixa(string id, CaixaSalvarModel model)
        {
            ValidarModelCaixa(model);
            var entity = ObterCaixaAtivo(id);
            ValidarLoja(model.LojaId);
            ValidarDuplicidadeCaixa(model, id);

            entity.LojaId = model.LojaId;
            entity.Descricao = model.Descricao!.Trim();
            entity.UpdatedAt = DateTime.UtcNow;
            entity.SyncStatus = "PENDING";

            _contextLocal.Caixa.Update(entity);
            RegistrarSyncLog("CAIXA", entity.Id, "UPDATE", entity);
            _contextLocal.SaveChanges();
            return entity;
        }

        public void DeletarCaixa(string id)
        {
            var entity = ObterCaixaAtivo(id);
            entity.Ativo = "F";
            entity.UpdatedAt = DateTime.UtcNow;
            entity.SyncStatus = "PENDING";

            _contextLocal.Caixa.Update(entity);
            RegistrarSyncLog("CAIXA", entity.Id, "DELETE", entity);
            _contextLocal.SaveChanges();
        }

        public CaixaMovimentacao IncluirMovimentacao(CaixaMovimentacaoSalvarModel model)
        {
            ValidarModelMovimentacao(model);
            _ = ObterCaixaAtivo(model.CaixaId);
            _ = ObterTipoValor(model.TipoValorCaixaId);

            var entity = new CaixaMovimentacao
            {
                Id = Guid.NewGuid().ToString("D"),
                CaixaId = model.CaixaId,
                TipoValorCaixaId = model.TipoValorCaixaId,
                Valor = model.Valor,
                DataCadastro = DateTime.Now,
                DataCompetencia = model.DataCompetencia,
                Descricao = model.Descricao,
                DataRealizacao = model.DataRealizacao,
                AnexoNome = model.AnexoNome,
                AnexoContentType = model.AnexoContentType,
                AnexoArquivo = model.AnexoArquivo,
                NomeFuncionario = model.NomeFuncionario,
                Ativo = "V",
                CreatedAt = DateTime.UtcNow,
                SyncStatus = "PENDING"
            };

            _contextLocal.CaixaMovimentacao.Add(entity);
            RegistrarSyncLog("CAIXA_MOVIMENTACAO", entity.Id, "INSERT", entity);
            _contextLocal.SaveChanges();
            return entity;
        }

        public CaixaMovimentacao AlterarMovimentacao(string id, CaixaMovimentacaoSalvarModel model)
        {
            ValidarModelMovimentacao(model);
            var entity = ObterMovimentacaoAtiva(id);
            _ = ObterCaixaAtivo(model.CaixaId);
            _ = ObterTipoValor(model.TipoValorCaixaId);

            entity.CaixaId = model.CaixaId;
            entity.TipoValorCaixaId = model.TipoValorCaixaId;
            entity.Valor = model.Valor;
            entity.DataCompetencia = model.DataCompetencia;
            entity.Descricao = model.Descricao;
            entity.DataRealizacao = model.DataRealizacao;
            entity.AnexoNome = model.AnexoNome;
            entity.AnexoContentType = model.AnexoContentType;
            entity.AnexoArquivo = model.AnexoArquivo;
            entity.NomeFuncionario = model.NomeFuncionario;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.SyncStatus = "PENDING";

            _contextLocal.CaixaMovimentacao.Update(entity);
            RegistrarSyncLog("CAIXA_MOVIMENTACAO", entity.Id, "UPDATE", entity);
            _contextLocal.SaveChanges();
            return entity;
        }

        public void DeletarMovimentacao(string id)
        {
            var entity = ObterMovimentacaoAtiva(id);
            entity.Ativo = "F";
            entity.UpdatedAt = DateTime.UtcNow;
            entity.SyncStatus = "PENDING";

            _contextLocal.CaixaMovimentacao.Update(entity);
            RegistrarSyncLog("CAIXA_MOVIMENTACAO", entity.Id, "DELETE", entity);
            _contextLocal.SaveChanges();
        }

        public List<Caixa> ListarCaixas(string lojaId)
        {
            if (string.IsNullOrWhiteSpace(lojaId)) throw new ArgumentException("LojaId e obrigatorio.");
            return _contextLocal.Caixa.Where(c => c.LojaId == lojaId && c.Ativo == "V")
                .OrderBy(c => c.Descricao).ToList();
        }

        public List<int> ListarDias(int ano, int mes)
        {
            if (mes < 1 || mes > 12)
                throw new ArgumentOutOfRangeException(nameof(mes), "O mes deve estar entre 1 e 12.");
            return Enumerable.Range(1, DateTime.DaysInMonth(ano, mes)).ToList();
        }

        public List<TipoValorCaixa> ListarTiposValor()
            => _contextLocal.TipoValorCaixa.Where(t => !t.IsDeleted).OrderBy(t => t.Descricao).ToList();

        public TipoValorCaixa CriarTipoValor(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("Descricao e obrigatoria.");

            var entity = new TipoValorCaixa
            {
                Id = Guid.NewGuid().ToString("D"),
                Descricao = descricao.Trim(),
                CreatedAt = DateTime.UtcNow,
                SyncStatus = "PENDING"
            };

            _contextLocal.TipoValorCaixa.Add(entity);
            RegistrarSyncLog("TIPO_VALOR_CAIXA", entity.Id, "INSERT", entity);
            _contextLocal.SaveChanges();
            return entity;
        }

        public TipoValorCaixa AlterarTipoValor(string id, string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("Descricao e obrigatoria.");

            var entity = ObterTipoValor(id);
            entity.Descricao = descricao.Trim();
            entity.UpdatedAt = DateTime.UtcNow;
            entity.SyncStatus = "PENDING";

            _contextLocal.TipoValorCaixa.Update(entity);
            RegistrarSyncLog("TIPO_VALOR_CAIXA", entity.Id, "UPDATE", entity);
            _contextLocal.SaveChanges();
            return entity;
        }

        public void DeletarTipoValor(string id)
        {
            var entity = ObterTipoValor(id);
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.SyncStatus = "PENDING";

            _contextLocal.TipoValorCaixa.Update(entity);
            RegistrarSyncLog("TIPO_VALOR_CAIXA", entity.Id, "DELETE", entity);
            _contextLocal.SaveChanges();
        }

        // ── Privados ──────────────────────────────────────────────────────────

        private void ValidarData(string lojaId, DateTime data)
        {
            if (string.IsNullOrWhiteSpace(lojaId)) throw new ArgumentException("LojaId e obrigatorio.");
            if (data == DateTime.MinValue) throw new ArgumentException("Data invalida.");
        }

        private void ValidarModelCaixa(CaixaSalvarModel model)
        {
            if (string.IsNullOrWhiteSpace(model.LojaId)) throw new ArgumentException("LojaId e obrigatorio.");
            if (string.IsNullOrWhiteSpace(model.Descricao)) throw new ArgumentException("Descricao e obrigatoria.");
        }

        private void ValidarModelMovimentacao(CaixaMovimentacaoSalvarModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CaixaId))          throw new ArgumentException("CaixaId e obrigatorio.");
            if (string.IsNullOrWhiteSpace(model.TipoValorCaixaId)) throw new ArgumentException("TipoValorCaixaId e obrigatorio.");
            if (model.Valor <= 0)                                    throw new ArgumentException("Valor deve ser positivo.");
            if (!model.DataCompetencia.HasValue)                     throw new ArgumentException("DataCompetencia e obrigatoria.");
        }

        private void ValidarLoja(string lojaId)
        {
            if (!_contextLocal.Lojas.Any(x => x.Id == lojaId && !x.IsDeleted))
                throw new ArgumentException("Loja nao encontrada.");
        }

        private void ValidarDuplicidadeCaixa(CaixaSalvarModel model, string? idAtual)
        {
            var desc = model.Descricao!.Trim();
            var query = _contextLocal.Caixa
                .Where(x => x.LojaId == model.LojaId && x.Descricao == desc && x.Ativo == "V");

            if (!string.IsNullOrWhiteSpace(idAtual))
                query = query.Where(x => x.Id != idAtual);

            if (query.Any())
                throw new ArgumentException("Ja existe caixa cadastrado para esta loja com esta descricao.");
        }

        private Caixa ObterCaixaAtivo(string id)
        {
            return _contextLocal.Caixa.FirstOrDefault(x => x.Id == id && x.Ativo == "V")
                ?? throw new KeyNotFoundException("Caixa nao encontrado.");
        }

        private CaixaMovimentacao ObterMovimentacaoAtiva(string id)
        {
            return _contextLocal.CaixaMovimentacao.FirstOrDefault(x => x.Id == id && x.Ativo == "V")
                ?? throw new KeyNotFoundException("Movimentacao nao encontrada.");
        }

        private TipoValorCaixa ObterTipoValor(string id)
        {
            return _contextLocal.TipoValorCaixa.FirstOrDefault(x => x.Id == id && !x.IsDeleted)
                ?? throw new ArgumentException("Tipo de valor invalido.");
        }

        private void RegistrarSyncLog(string tabela, string registroId, string operacao, object payload)
        {
            try
            {
                _contextLocal.SyncLogs.Add(new SyncLog
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Tabela = tabela,
                    RegistroId = registroId,
                    Operacao = operacao,
                    Status = "PENDING",
                    Payload = JsonSerializer.Serialize(payload),
                    CreatedAt = DateTime.UtcNow
                });
            }
            catch { /* sync log nao deve bloquear operacao principal */ }
        }
    }
}
