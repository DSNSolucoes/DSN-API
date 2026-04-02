using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity; 
using Microsoft.AspNetCore.Mvc;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


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
            var retorno = _ContextLocal.NCMs.ToList();
            return Ok(retorno);
        }

        [HttpPost]
        public async Task<ActionResult<Ncm>> PostNcm([FromBody] Ncm ncm)
        {
            _ContextLocal.NCMs.Add(ncm);
            await _ContextLocal.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNcms), new { id = ncm.Id }, ncm);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNcm(int id)
        {
            var ncm = await _ContextLocal.NCMs.FindAsync(id);
            if (ncm == null)
            {
                return NotFound();
            }

            _ContextLocal.NCMs.Remove(ncm);
            await _ContextLocal.SaveChangesAsync();

            return NoContent();
        }
    }
}