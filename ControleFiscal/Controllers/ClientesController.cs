using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using ControleFiscal.Utils;

using Microsoft.AspNetCore.Mvc;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClienteController : ControllerBase
    {

        private readonly ILogger<ClienteController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public ClienteController(ILogger<ClienteController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet]
        public ActionResult<List<Clientes>> Obter([FromQuery] int lojaId)
        {
            var nomeLoja = "";
            try
            {
                nomeLoja = _Context.ConexaoCliente(lojaId, _ContextLocal);
                return _Context.Clientes.Where(x => x.Deletado != "V").Take(100).ToList();
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }
    }
}