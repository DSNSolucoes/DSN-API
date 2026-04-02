using ControleFiscal.Infrastructure.Sql; 
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using Microsoft.AspNetCore.Mvc;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuncionarioController : ControllerBase
    {

        private readonly ILogger<FuncionarioController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public FuncionarioController(ILogger<FuncionarioController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }


        [HttpGet]
        public ActionResult<List<FuncionarioCaixaDTO>> Reforma([FromQuery] int empresaId)
        {
            try
            {
                  var funcionario = new FuncionarioCaixaDTO()
                  {
                      Id = 1,
                      Nome = "Teste"
                  };  

                var retorno = new List<FuncionarioCaixaDTO>();
                retorno.Add(funcionario);

                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + e.StackTrace);
            }
        }
    }
}