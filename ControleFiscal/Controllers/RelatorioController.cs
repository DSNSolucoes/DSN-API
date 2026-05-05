using ControleFiscal.Infrastructure.Sql; 
using Microsoft.AspNetCore.Mvc;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatorioController : ControllerBase
    {

        private readonly ILogger<RelatorioController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public RelatorioController(ILogger<RelatorioController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet]
        public List<Pedido> Obter([FromBody] int lojaId)
        {

            var loja = _ContextLocal.Empresas.FirstOrDefault(x => x.Id == lojaId);

            if (loja != null)
            {
                _Context.ConexaoCliente(loja.Caminho, loja.Host);
                var lista = _Context.Pedido.Where(x => x.Cancelado != "V").Take(100).ToList();
                return lista;
            }

            throw new Exception("Loja não encontrada");

        }
    }
}