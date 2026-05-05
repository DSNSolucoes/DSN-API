using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Utils;

using Microsoft.AspNetCore.Mvc;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfiguracoesController : ControllerBase
    {

        private readonly ILogger<ConfiguracoesController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public ConfiguracoesController(ILogger<ConfiguracoesController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet]
        public ActionResult<List<NfceConfig>> ObterNFCe([FromQuery] int lojaId)
        {
            var nomeLoja = "";
            try
            {
                Empresa? loja = _ContextLocal.Empresas.FirstOrDefault(x => x.Id == lojaId);

                nomeLoja = loja?.Nome;

                _Context.ConexaoCliente(loja?.Caminho ?? "", loja?.Host ?? "");

                var lista = _Context.NfceConfig.ToList();

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }
    }
}