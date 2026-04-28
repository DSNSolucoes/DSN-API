using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NCMController : ControllerBase
    {
        private readonly ILogger<NCMController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public NCMController(ILogger<NCMController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet]
        public ActionResult<List<Ncm>> GetNcms()
        {
            var retorno = _ContextLocal.NCMs.Where(n => !n.IsDeleted).ToList();
            return Ok(retorno);
        }

        [HttpPost]
        public async Task<ActionResult<Ncm>> PostNcm([FromBody] Ncm ncm)
        {
            ncm.Id = Guid.NewGuid().ToString("D");
            ncm.CreatedAt = DateTime.UtcNow;
            ncm.SyncStatus = "PENDING";
            ncm.IsDeleted = false;

            _ContextLocal.NCMs.Add(ncm);

            _ContextLocal.SyncLogs.Add(new SyncLog
            {
                Id = Guid.NewGuid().ToString("D"),
                Tabela = "NCM",
                RegistroId = ncm.Id,
                Operacao = "INSERT",
                Status = "PENDING",
                Payload = JsonSerializer.Serialize(ncm),
                CreatedAt = DateTime.UtcNow
            });

            await _ContextLocal.SaveChangesAsync();
            return CreatedAtAction(nameof(GetNcms), new { id = ncm.Id }, ncm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNcm(string id)
        {
            var ncm = await _ContextLocal.NCMs.FindAsync(id);
            if (ncm == null) return NotFound();

            ncm.IsDeleted = true;
            ncm.UpdatedAt = DateTime.UtcNow;
            ncm.SyncStatus = "PENDING";

            _ContextLocal.NCMs.Update(ncm);

            _ContextLocal.SyncLogs.Add(new SyncLog
            {
                Id = Guid.NewGuid().ToString("D"),
                Tabela = "NCM",
                RegistroId = ncm.Id,
                Operacao = "DELETE",
                Status = "PENDING",
                Payload = JsonSerializer.Serialize(ncm),
                CreatedAt = DateTime.UtcNow
            });

            await _ContextLocal.SaveChangesAsync();
            return NoContent();
        }
    }
}
