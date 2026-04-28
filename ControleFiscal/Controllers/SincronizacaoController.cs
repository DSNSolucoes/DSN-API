using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Domain.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SincronizacaoController : ControllerBase
    {
        private readonly ISyncService _syncService;
        private readonly ILogger<SincronizacaoController> _logger;

        public SincronizacaoController(ISyncService syncService, ILogger<SincronizacaoController> logger)
        {
            _syncService = syncService;
            _logger = logger;
        }

        /// <summary>
        /// Executa ciclo completo de sincronização local ↔ nuvem.
        /// </summary>
        [HttpPost("sincronizar")]
        public async Task<ActionResult<SyncResultDto>> Sincronizar(CancellationToken ct)
        {
            try
            {
                var result = await _syncService.SincronizarAsync(ct);
                return result.Sucesso ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no endpoint de sincronização.");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Retorna status atual da sincronização (last sync, pendentes, conectividade).
        /// </summary>
        [HttpGet("status")]
        public async Task<ActionResult<SyncStatusDto>> Status()
        {
            try { return Ok(await _syncService.ObterStatusAsync()); }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        /// <summary>
        /// Define a URL da API na nuvem.
        /// </summary>
        [HttpPut("configurar")]
        public async Task<IActionResult> Configurar([FromBody] SyncConfigurarModel model)
        {
            if (string.IsNullOrWhiteSpace(model?.UrlNuvem))
                return BadRequest("UrlNuvem é obrigatória.");

            await _syncService.ConfigurarUrlNuvemAsync(model.UrlNuvem);
            return Ok(new { mensagem = "URL configurada com sucesso." });
        }

        /// <summary>
        /// Lista logs de sincronização paginados.
        /// </summary>
        [HttpGet("log")]
        public IActionResult Log([FromQuery] int pagina = 0, [FromQuery] int tamanho = 50)
        {
            var logs = _syncService.ObterLogs(pagina, tamanho);
            return Ok(logs);
        }
    }
}
