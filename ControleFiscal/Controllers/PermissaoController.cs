using ControleFiscal.Domain.Model;
using ControleFiscal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissaoController : ControllerBase
    {
        private readonly ILogger<PermissaoController> _logger;
        private readonly IPermissaoService _permissaoService;

        public PermissaoController(ILogger<PermissaoController> logger, IPermissaoService permissaoService)
        {
            _logger = logger;
            _permissaoService = permissaoService;
        }

        // ── Permissões ────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Listar()
        {
            try { return Ok(_permissaoService.ListarPermissoes()); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar permissões."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public IActionResult Criar([FromBody] PermissaoSalvarModel model)
        {
            try { return Ok(_permissaoService.CriarPermissao(model.Nome ?? "", model.Descricao ?? "")); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar permissão."); return BadRequest(e.Message); }
        }

        [HttpPut("{id}")]
        public IActionResult Alterar(int id, [FromBody] PermissaoSalvarModel model)
        {
            try { return Ok(_permissaoService.AlterarPermissao(id, model.Nome ?? "", model.Descricao ?? "")); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar permissão."); return BadRequest(e.Message); }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try { _permissaoService.DeletarPermissao(id); return Ok("Permissão removida."); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao deletar permissão."); return BadRequest(e.Message); }
        }

        // ── Usuários ──────────────────────────────────────────────────────────

        [HttpGet("usuarios")]
        public IActionResult ListarUsuarios()
        {
            try
            {
                var usuarios = _permissaoService.ListarUsuarios()
                    .Select(u => new { u.Id, u.Nome, u.Login })
                    .ToList();
                return Ok(usuarios);
            }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar usuários."); return BadRequest(e.Message); }
        }

        // ── Vínculo usuário-permissão ─────────────────────────────────────────

        [HttpGet("usuario/{usuarioId}")]
        public IActionResult ListarPermissoesDoUsuario(int usuarioId)
        {
            try { return Ok(_permissaoService.ListarPermissoesDoUsuario(usuarioId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar permissões do usuário."); return BadRequest(e.Message); }
        }

        [HttpPost("usuario/{usuarioId}/vincular")]
        public IActionResult Vincular(int usuarioId, [FromBody] VincularPermissaoModel model)
        {
            try { return Ok(_permissaoService.VincularPermissao(usuarioId, model.PermissaoId)); }
            catch (InvalidOperationException e) { return Conflict(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao vincular permissão."); return BadRequest(e.Message); }
        }

        [HttpDelete("vinculo/{permissaoUsuarioId}")]
        public IActionResult Desvincular(int permissaoUsuarioId)
        {
            try { _permissaoService.DesvincularPermissao(permissaoUsuarioId); return Ok("Vínculo removido."); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao desvincular permissão."); return BadRequest(e.Message); }
        }
    }
}
