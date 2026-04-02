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

        public CaixaController(
            ILogger<CaixaController> logger,
            ICaixaService caixaService)
        {
            _logger = logger;
            _caixaService = caixaService;
        }

        [HttpGet]
        public ActionResult<List<CaixaDTO>> Listar([FromQuery] int lojaId,
                                                   [FromQuery] DateTime data)
        {
            try
            {
                var retorno = _caixaService.Listar(lojaId, data.Date);
                return Ok(retorno);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao listar caixa.");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("resumo-mensal")]
        public ActionResult<List<CaixaResumoMensalDTO>> ListarResumoMensal( [FromQuery] int lojaId,
                                                                            [FromQuery] int ano,
                                                                            [FromQuery] int mes)
        {
            try
            {
                var retorno = _caixaService.ListarResumoMensal(lojaId, ano, mes);
                return Ok(retorno);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao listar resumo mensal do caixa.");
                return BadRequest(e.Message);
            }
        }

        [HttpPost("caixa")]
        public IActionResult IncluirCaixa([FromBody] CaixaSalvarModel model)
        {
            try
            {
                var entity = _caixaService.IncluirCaixa(model);
                return Ok(entity);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao incluir caixa.");
                return BadRequest(e.Message);
            }
        }

        [HttpPut("caixa/{id}")]
        public IActionResult AlterarCaixa(int id, [FromBody] CaixaSalvarModel model)
        {
            try
            {
                var entity = _caixaService.AlterarCaixa(id, model);
                return Ok(entity);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao alterar caixa.");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("caixa/{id}")]
        public IActionResult DeletarCaixa(int id)
        {
            try
            {
                _caixaService.DeletarCaixa(id);
                return Ok("Caixa removido com sucesso.");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao deletar caixa.");
                return BadRequest(e.Message);
            }
        }

        [HttpPost("movimentacao")]
        public IActionResult IncluirMovimentacao([FromBody] CaixaMovimentacaoSalvarModel model)
        {
            try
            {
                var entity = _caixaService.IncluirMovimentacao(model);
                return Ok(entity);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao incluir movimentação.");
                return BadRequest(e.Message);
            }
        }

        [HttpPut("movimentacao/{id}")]
        public IActionResult AlterarMovimentacao(int id, [FromBody] CaixaMovimentacaoSalvarModel model)
        {
            try
            {
                var entity = _caixaService.AlterarMovimentacao(id, model);
                return Ok(entity);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao alterar movimentação.");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("movimentacao/{id}")]
        public IActionResult DeletarMovimentacao(int id)
        {
            try
            {
                _caixaService.DeletarMovimentacao(id);
                return Ok("Movimentação removida com sucesso.");
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao deletar movimentação.");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ListarDias")]
        public ActionResult<List<int>> ListarDias([FromQuery] int anoCompetencia,
                                                  [FromQuery] int mesCompetencia)
        {
            try
            {
                var dias = _caixaService.ListarDias(anoCompetencia, mesCompetencia);
                return Ok(dias);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao listar dias.");
                return BadRequest(e.Message);
            }
        }

        [HttpGet("tipovalor")]
        public ActionResult<List<TipoValorCaixa>> ListarTiposValor()
        {
            try
            {
                var tipos = _caixaService.ListarTiposValor();
                return Ok(tipos);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao listar tipos de valor.");
                return BadRequest(e.Message);
            }
        }

        [HttpPost("tipovalor")]
        public IActionResult CriarTipoValor([FromBody] TipoValorCaixaSalvarModel model)
        {
            try
            {
                var entity = _caixaService.CriarTipoValor(model.Descricao ?? "");
                return Ok(entity);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao criar tipo de valor.");
                return BadRequest(e.Message);
            }
        }
    }
}