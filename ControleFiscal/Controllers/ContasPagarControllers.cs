using ControleFiscal.Domain.DTO.ContasPagar;
using ControleFiscal.Domain.Model.ContasPagar;
using ControleFiscal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleFiscal.Controllers
{
    // ── Fornecedores CP ──────────────────────────────────────────────────────
    [ApiController]
    [Route("api/empresas/{empresaId}/fornecedores-cp")]
    public class FornecedoresCPController : ControllerBase
    {
        private readonly IFornecedorCPService _service;
        private readonly ILogger<FornecedoresCPController> _logger;
        public FornecedoresCPController(IFornecedorCPService service, ILogger<FornecedoresCPController> logger)
        { _service = service; _logger = logger; }

        [HttpGet]
        public ActionResult<List<FornecedorDTO>> Listar(int empresaId)
        {
            try { return Ok(_service.Listar(empresaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar fornecedores."); return BadRequest(e.Message); }
        }

        [HttpGet("{cdFornecedor:int}")]
        public ActionResult<FornecedorDTO> Obter(int empresaId, int cdFornecedor)
        {
            try { return Ok(_service.Obter(empresaId, cdFornecedor)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter fornecedor."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public ActionResult Criar(int empresaId, [FromBody] FornecedorSalvarModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                var entidade = _service.Criar(model);
                return Ok(new { entidade.CdFornecedor });
            }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar fornecedor."); return BadRequest(e.Message); }
        }

        [HttpPut("{cdFornecedor:int}")]
        public ActionResult Alterar(int empresaId, int cdFornecedor, [FromBody] FornecedorSalvarModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                _service.Alterar(cdFornecedor, model);
                return Ok();
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar fornecedor."); return BadRequest(e.Message); }
        }

        [HttpDelete("{cdFornecedor:int}")]
        public ActionResult Inativar(int cdFornecedor)
        {
            try { _service.Inativar(cdFornecedor); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao inativar fornecedor."); return BadRequest(e.Message); }
        }
    }

    // ── Categorias CP ────────────────────────────────────────────────────────
    [ApiController]
    [Route("api/empresas/{empresaId}/categorias-contas-pagar")]
    public class CategoriasCPController : ControllerBase
    {
        private readonly ICategoriaContaPagarService _service;
        private readonly ILogger<CategoriasCPController> _logger;
        public CategoriasCPController(ICategoriaContaPagarService service, ILogger<CategoriasCPController> logger)
        { _service = service; _logger = logger; }

        [HttpGet]
        public ActionResult<List<CategoriaContaPagarDTO>> Listar(int empresaId)
        {
            try { return Ok(_service.Listar(empresaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar categorias."); return BadRequest(e.Message); }
        }

        [HttpGet("{id:int}")]
        public ActionResult<CategoriaContaPagarDTO> Obter(int empresaId, int id)
        {
            try { return Ok(_service.Obter(empresaId, id)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter categoria."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public ActionResult Criar(int empresaId, [FromBody] CategoriaContaPagarSalvarModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                var entidade = _service.Criar(model);
                return Ok(new { entidade.Id });
            }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar categoria."); return BadRequest(e.Message); }
        }

        [HttpPut("{id:int}")]
        public ActionResult Alterar(int empresaId, int id, [FromBody] CategoriaContaPagarSalvarModel model)
        {
            try { model.IdEmpresa = empresaId; _service.Alterar(id, model); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar categoria."); return BadRequest(e.Message); }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Inativar(int id)
        {
            try { _service.Inativar(id); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao inativar categoria."); return BadRequest(e.Message); }
        }
    }

    // ── Centros de Custo CP ──────────────────────────────────────────────────
    [ApiController]
    [Route("api/empresas/{empresaId}/centros-custo-cp")]
    public class CentrosCustoCPController : ControllerBase
    {
        private readonly ICentroCustoCPService _service;
        private readonly ILogger<CentrosCustoCPController> _logger;
        public CentrosCustoCPController(ICentroCustoCPService service, ILogger<CentrosCustoCPController> logger)
        { _service = service; _logger = logger; }

        [HttpGet]
        public ActionResult<List<CentroCustoCPDTO>> Listar(int empresaId)
        {
            try { return Ok(_service.Listar(empresaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar centros de custo."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public ActionResult Criar(int empresaId, [FromBody] CentroCustoCPSalvarModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                var entidade = _service.Criar(model);
                return Ok(new { entidade.Id });
            }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar centro de custo."); return BadRequest(e.Message); }
        }

        [HttpPut("{id:int}")]
        public ActionResult Alterar(int empresaId, int id, [FromBody] CentroCustoCPSalvarModel model)
        {
            try { model.IdEmpresa = empresaId; _service.Alterar(id, model); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar centro de custo."); return BadRequest(e.Message); }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Inativar(int id)
        {
            try { _service.Inativar(id); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao inativar centro de custo."); return BadRequest(e.Message); }
        }
    }

    // ── Contas a Pagar ───────────────────────────────────────────────────────
    [ApiController]
    [Route("api/empresas/{empresaId}/contas-pagar")]
    public class ContasPagarController : ControllerBase
    {
        private readonly IContaPagarService _service;
        private readonly ILogger<ContasPagarController> _logger;
        public ContasPagarController(IContaPagarService service, ILogger<ContasPagarController> logger)
        { _service = service; _logger = logger; }

        [HttpGet]
        public ActionResult<List<ContaPagarDTO>> Listar(int empresaId, [FromQuery] FiltroContasPagarModel filtro)
        {
            try { filtro.IdEmpresa = empresaId; return Ok(_service.Listar(filtro)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar contas."); return BadRequest(e.Message); }
        }

        [HttpGet("{id:int}")]
        public ActionResult<ContaPagarDTO> Obter(int empresaId, int id)
        {
            try { return Ok(_service.Obter(empresaId, id)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter conta."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public ActionResult Criar(int empresaId, [FromBody] ContaPagarSalvarModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                var entidade = _service.Criar(model);
                return Ok(new { entidade.Id });
            }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar conta."); return BadRequest(e.Message); }
        }

        [HttpPost("parcelas")]
        public ActionResult GerarParcelas(int empresaId, [FromBody] GerarParcelasModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                var parcelas = _service.GerarParcelas(model);
                return Ok(parcelas.Select(p => new { p.Id, p.NumeroParcela, p.TotalParcelas, p.DataVencimento, p.ValorTotal }));
            }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao gerar parcelas."); return BadRequest(e.Message); }
        }

        [HttpPut("{id:int}")]
        public ActionResult Alterar(int empresaId, int id, [FromBody] ContaPagarSalvarModel model)
        {
            try { model.IdEmpresa = empresaId; _service.Alterar(id, model); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar conta."); return BadRequest(e.Message); }
        }

        [HttpPost("{id:int}/cancelar")]
        public ActionResult Cancelar(int empresaId, int id, [FromBody] CancelarContaModel model)
        {
            try { model.IdEmpresa = empresaId; _service.Cancelar(id, model); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao cancelar conta."); return BadRequest(e.Message); }
        }

        [HttpPost("{id:int}/reabrir")]
        public ActionResult Reabrir(int empresaId, int id, [FromBody] ReobrirContaModel model)
        {
            try { model.IdEmpresa = empresaId; _service.Reabrir(id, model); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao reabrir conta."); return BadRequest(e.Message); }
        }
    }

    // ── Pagamentos ───────────────────────────────────────────────────────────
    [ApiController]
    [Route("api/empresas/{empresaId}/contas-pagar/{contaId:int}/pagamentos")]
    public class PagamentosContaPagarController : ControllerBase
    {
        private readonly IPagamentoContaPagarService _service;
        private readonly ILogger<PagamentosContaPagarController> _logger;
        public PagamentosContaPagarController(IPagamentoContaPagarService service, ILogger<PagamentosContaPagarController> logger)
        { _service = service; _logger = logger; }

        [HttpGet]
        public ActionResult<List<PagamentoContaPagarDTO>> Listar(int empresaId, int contaId)
        {
            try { return Ok(_service.ListarPorConta(empresaId, contaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar pagamentos."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public ActionResult Registrar(int empresaId, int contaId, [FromBody] RegistrarPagamentoModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                var entidade = _service.Registrar(contaId, model);
                return Ok(new { entidade.Id });
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao registrar pagamento."); return BadRequest(e.Message); }
        }
    }

    // ── Estorno de Pagamento ─────────────────────────────────────────────────
    [ApiController]
    [Route("api/empresas/{empresaId}/pagamentos-contas-pagar")]
    public class EstornoPagamentoCPController : ControllerBase
    {
        private readonly IPagamentoContaPagarService _service;
        private readonly ILogger<EstornoPagamentoCPController> _logger;
        public EstornoPagamentoCPController(IPagamentoContaPagarService service, ILogger<EstornoPagamentoCPController> logger)
        { _service = service; _logger = logger; }

        [HttpPost("{pagamentoId:int}/estornar")]
        public ActionResult Estornar(int empresaId, int pagamentoId, [FromBody] EstornarPagamentoModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                _service.Estornar(pagamentoId, model);
                return Ok();
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao estornar pagamento."); return BadRequest(e.Message); }
        }
    }

    // ── Recorrências ─────────────────────────────────────────────────────────
    [ApiController]
    [Route("api/empresas/{empresaId}/contas-pagar-recorrentes")]
    public class ContasPagarRecorrentesController : ControllerBase
    {
        private readonly IContaPagarRecorrenteService _service;
        private readonly ILogger<ContasPagarRecorrentesController> _logger;
        public ContasPagarRecorrentesController(IContaPagarRecorrenteService service, ILogger<ContasPagarRecorrentesController> logger)
        { _service = service; _logger = logger; }

        [HttpGet]
        public ActionResult<List<ContaPagarRecorrenteDTO>> Listar(int empresaId)
        {
            try { return Ok(_service.Listar(empresaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar recorrências."); return BadRequest(e.Message); }
        }

        [HttpGet("{id:int}")]
        public ActionResult<ContaPagarRecorrenteDTO> Obter(int empresaId, int id)
        {
            try { return Ok(_service.Obter(empresaId, id)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter recorrência."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public ActionResult Criar(int empresaId, [FromBody] ContaPagarRecorrenteSalvarModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                var entidade = _service.Criar(model);
                return Ok(new { entidade.Id });
            }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar recorrência."); return BadRequest(e.Message); }
        }

        [HttpPut("{id:int}")]
        public ActionResult Alterar(int empresaId, int id, [FromBody] ContaPagarRecorrenteSalvarModel model)
        {
            try { model.IdEmpresa = empresaId; _service.Alterar(id, model); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar recorrência."); return BadRequest(e.Message); }
        }

        [HttpPost("{id:int}/cancelar")]
        public ActionResult Cancelar(int empresaId, int id, [FromQuery] int? idResponsavel)
        {
            try { _service.Cancelar(id, idResponsavel); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao cancelar recorrência."); return BadRequest(e.Message); }
        }

        [HttpPost("{id:int}/gerar-contas")]
        public ActionResult GerarContas(int empresaId, int id, [FromBody] GerarContasRecorrentesModel model)
        {
            try
            {
                model.IdEmpresa = empresaId;
                var contas = _service.GerarContas(id, model);
                return Ok(new { geradas = contas.Count });
            }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao gerar contas."); return BadRequest(e.Message); }
        }
    }

    // ── Dashboard ────────────────────────────────────────────────────────────
    [ApiController]
    [Route("api/empresas/{empresaId}/dashboard-contas-pagar")]
    public class DashboardContasPagarController : ControllerBase
    {
        private readonly IDashboardContasPagarService _service;
        private readonly ILogger<DashboardContasPagarController> _logger;
        public DashboardContasPagarController(IDashboardContasPagarService service, ILogger<DashboardContasPagarController> logger)
        { _service = service; _logger = logger; }

        [HttpGet]
        public ActionResult<DashboardContasPagarDTO> Obter(int empresaId)
        {
            try { return Ok(_service.Obter(empresaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter dashboard."); return BadRequest(e.Message); }
        }
    }
}
