using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
   // [Authorize]
    public class CombosController : ControllerBase
    {

        private readonly ILogger<ProdutoController> _logger;
        private readonly ContextLocalContext _Context;

        public CombosController(ILogger<ProdutoController> logger, ContextLocalContext Context)
        {
            _logger = logger;
            _Context = Context;
        }

        [HttpGet("ObterLojas")]
        [Authorize]
        public ActionResult<List<Empresa>> ObterLojas()
        {
            try
            {
                var usuario = User!.Identity!.Name ?? string.Empty;

                if (Debugger.IsAttached && string.IsNullOrEmpty(usuario))
                {
                    usuario = "Douglas";
                }

                List<Empresa> resultado; 

                var Configuracao = _Context.Logins?.FirstOrDefault(x => (x.Nome ?? "").ToUpper() == usuario.ToUpper());            

                if (string.IsNullOrEmpty(Configuracao?.EmpresaId))
                {
                    resultado = _Context.Empresas.OrderBy(x => x.Nome).ToList();
                }
                else
                {
                    var listaEmpresa = Configuracao.EmpresaId.Split(",").Select(x => int.Parse(x)).ToList();
                    resultado = _Context.Empresas.Where(x => listaEmpresa.Contains(x.Id)).OrderBy(x => x.Nome).ToList();
                }

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, "local"));
            }

        }

        [HttpGet("ObterNCM")]
        public ActionResult<List<Ncm>> ObterNCM()
        {
            try
            { 
                var resultado = _Context.NCMs.ToList();

                return Ok(resultado);
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, "local"));
            }

        }


    }
}