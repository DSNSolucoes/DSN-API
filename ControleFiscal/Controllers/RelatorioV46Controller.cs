using ControleFiscal.Infrastructure.Sql;

using Microsoft.AspNetCore.Mvc;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;



namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatorioV46Controller : ControllerBase
    {

        private readonly ILogger<RelatorioV46Controller> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public RelatorioV46Controller(ILogger<RelatorioV46Controller> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet]
        public List<Pedido> Obter([FromBody] int lojaId)
        {

            var loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);

            _Context.ConexaoCliente(loja?.Caminho, loja?.Host);

            var lista = _Context.Pedido.Where(x => x.Cancelado != "V").Take(100).ToList();


            return lista;

        }
    }
}