using ControleFiscal.Infrastructure.Sql;

using Microsoft.AspNetCore.Mvc; 
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<ProdutoController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public LoginController(ILogger<ProdutoController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpPost("login")]        
        public ActionResult<LoginRetornoDTO> Obter([FromBody] LoginDTO login)
        {
            try
            {
                var resultado = _ContextLocal.Logins.FirstOrDefault(x => x.Login.ToUpper() == login.Login.ToUpper());

                if (resultado.Bloqueado == "V")
                {
                    return Forbid();
                }

                if (login.Senha == null || login.Senha == string.Empty)
                {
                    throw new Exception("Senha não pode ser vazia;");
                }


                if (LoginRetornoDTO.TentativaDeInvasao(login.Senha))
                {
                    resultado.Bloqueado = "V";
                    _ContextLocal.Logins.Update(resultado);

                    return Conflict("Tentativa de invasão detectada.");
                }

                if (resultado != null && resultado.Senha == login.Senha)
                {
                    var retorno = new LoginRetornoDTO(resultado);
                    return Ok(retorno);
                }

                return Unauthorized();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        [HttpPost("trocarsenha")]
        public ActionResult<LoginRetornoDTO> TrocarSenha([FromBody] RecuperarUsuarioDTO login)
        {
            try
            {               
                if (login.Login == string.Empty || login.Login == null)
                {
                    throw new Exception("Senha não pode ser vazia;");
                }

                var resultado = _ContextLocal.Logins.FirstOrDefault(x => x.Login.ToUpper() == login.Login.ToUpper());

                if (resultado != null && resultado.Senha == login.Senha && login.NovaSenha == login.ConfimarSenha)
                {
                    resultado.Senha = login.NovaSenha;
                    _ContextLocal.SaveChanges(); 
                    return Ok(new LoginRetornoDTO(resultado));
                }

                return Unauthorized();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}