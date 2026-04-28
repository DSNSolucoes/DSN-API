using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Domain.Model;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CaixaController : ControllerBase
    {
        private readonly ILogger<CaixaController> _logger;
        private readonly ICaixaService _caixaService;

        public CaixaController(ILogger<CaixaController> logger, ICaixaService caixaService)
        {
            _logger = logger;
            _caixaService = caixaService;
        }

        [HttpGet]
        public ActionResult<List<CaixaDTO>> Listar([FromQuery] string lojaId, [FromQuery] DateTime data)
        {
            try { return Ok(_caixaService.Listar(lojaId, data.Date)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar caixa."); return BadRequest(e.Message); }
        }

        [HttpGet("resumo-mensal")]
        public ActionResult<List<CaixaResumoMensalDTO>> ListarResumoMensal(
            [FromQuery] string lojaId, [FromQuery] int ano, [FromQuery] int mes)
        {
            try { return Ok(_caixaService.ListarResumoMensal(lojaId, ano, mes)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar resumo mensal."); return BadRequest(e.Message); }
        }

        [HttpPost("caixa")]
        public IActionResult IncluirCaixa([FromBody] CaixaSalvarModel model)
        {
            try { return Ok(_caixaService.IncluirCaixa(model)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao incluir caixa."); return BadRequest(e.Message); }
        }

        [HttpPut("caixa/{id}")]
        public IActionResult AlterarCaixa(string id, [FromBody] CaixaSalvarModel model)
        {
            try { return Ok(_caixaService.AlterarCaixa(id, model)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar caixa."); return BadRequest(e.Message); }
        }

        [HttpDelete("caixa/{id}")]
        public IActionResult DeletarCaixa(string id)
        {
            try { _caixaService.DeletarCaixa(id); return Ok("Caixa removido com sucesso."); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao deletar caixa."); return BadRequest(e.Message); }
        }

        [HttpPost("movimentacao")]
        public IActionResult IncluirMovimentacao([FromBody] CaixaMovimentacaoSalvarModel model)
        {
            try { return Ok(_caixaService.IncluirMovimentacao(model)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao incluir movimentacao."); return BadRequest(e.Message); }
        }

        [HttpPut("movimentacao/{id}")]
        public IActionResult AlterarMovimentacao(string id, [FromBody] CaixaMovimentacaoSalvarModel model)
        {
            try { return Ok(_caixaService.AlterarMovimentacao(id, model)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar movimentacao."); return BadRequest(e.Message); }
        }

        [HttpDelete("movimentacao/{id}")]
        public IActionResult DeletarMovimentacao(string id)
        {
            try { _caixaService.DeletarMovimentacao(id); return Ok("Movimentacao removida com sucesso."); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao deletar movimentacao."); return BadRequest(e.Message); }
        }

        [HttpGet("ListarDias")]
        public ActionResult<List<int>> ListarDias([FromQuery] int anoCompetencia, [FromQuery] int mesCompetencia)
        {
            try { return Ok(_caixaService.ListarDias(anoCompetencia, mesCompetencia)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar dias."); return BadRequest(e.Message); }
        }

        [HttpGet("tipovalor")]
        public ActionResult<List<TipoValorCaixa>> ListarTiposValor()
        {
            try { return Ok(_caixaService.ListarTiposValor()); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar tipos de valor."); return BadRequest(e.Message); }
        }

        [HttpPost("tipovalor")]
        public IActionResult CriarTipoValor([FromBody] TipoValorCaixaSalvarModel model)
        {
            try { return Ok(_caixaService.CriarTipoValor(model.Descricao ?? "")); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar tipo de valor."); return BadRequest(e.Message); }
        }

        [HttpPut("tipovalor/{id}")]
        public IActionResult AlterarTipoValor(string id, [FromBody] TipoValorCaixaSalvarModel model)
        {
            try { return Ok(_caixaService.AlterarTipoValor(id, model.Descricao ?? "")); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar tipo de valor."); return BadRequest(e.Message); }
        }

        [HttpDelete("tipovalor/{id}")]
        public IActionResult DeletarTipoValor(string id)
        {
            try { _caixaService.DeletarTipoValor(id); return Ok("Tipo de valor removido com sucesso."); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao deletar tipo de valor."); return BadRequest(e.Message); }
        }

        [HttpGet("caixas")]
        public ActionResult<List<Caixa>> ListarCaixas([FromQuery] string lojaId)
        {
            try { return Ok(_caixaService.ListarCaixas(lojaId)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar caixas."); return BadRequest(e.Message); }
        }
    }
}
