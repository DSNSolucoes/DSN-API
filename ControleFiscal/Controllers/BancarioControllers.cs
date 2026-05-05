using ControleFiscal.Domain.Model.Bancario;
using ControleFiscal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("api/bancos")]
    public class BancoController : ControllerBase
    {
        private readonly ILogger<BancoController> _logger;
        private readonly IBancoService _bancoService;

        public BancoController(ILogger<BancoController> logger, IBancoService bancoService)
        {
            _logger = logger;
            _bancoService = bancoService;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            try { return Ok(_bancoService.Listar()); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar bancos."); return BadRequest(e.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int id)
        {
            try { return Ok(_bancoService.Obter(id)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter banco."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public IActionResult Criar([FromBody] BancoSalvarModel model)
        {
            try { return Ok(_bancoService.Criar(model)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar banco."); return BadRequest(e.Message); }
        }

        [HttpPut("{id}")]
        public IActionResult Alterar(int id, [FromBody] BancoSalvarModel model)
        {
            try { return Ok(_bancoService.Alterar(id, model)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar banco."); return BadRequest(e.Message); }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try { _bancoService.Deletar(id); return NoContent(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao deletar banco."); return BadRequest(e.Message); }
        }
    }

    [ApiController]
    [Route("api/empresas/{empresaId}/contas-bancarias")]
    public class ContaBancariaController : ControllerBase
    {
        private readonly ILogger<ContaBancariaController> _logger;
        private readonly IContaBancariaService _contaService;

        public ContaBancariaController(ILogger<ContaBancariaController> logger, IContaBancariaService contaService)
        {
            _logger = logger;
            _contaService = contaService;
        }

        [HttpGet]
        public IActionResult Listar(int empresaId)
        {
            try { return Ok(_contaService.Listar(empresaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar contas."); return BadRequest(e.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int empresaId, int id)
        {
            try { return Ok(_contaService.Obter(empresaId, id)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter conta."); return BadRequest(e.Message); }
        }

        [HttpGet("{id}/saldo")]
        public IActionResult ObterSaldo(int empresaId, int id)
        {
            try { return Ok(_contaService.ObterSaldo(empresaId, id)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter saldo."); return BadRequest(e.Message); }
        }

        [HttpGet("saldo-empresa")]
        public IActionResult ObterSaldoEmpresa(int empresaId)
        {
            try { return Ok(_contaService.ObterSaldoEmpresa(empresaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter saldo empresa."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public IActionResult Criar(int empresaId, [FromBody] ContaBancariaSalvarModel model)
        {
            model.IdEmpresa = empresaId;
            try { return Ok(_contaService.Criar(model)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar conta."); return BadRequest(e.Message); }
        }

        [HttpPut("{id}")]
        public IActionResult Alterar(int empresaId, int id, [FromBody] ContaBancariaSalvarModel model)
        {
            model.IdEmpresa = empresaId;
            try { return Ok(_contaService.Alterar(id, model)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar conta."); return BadRequest(e.Message); }
        }

        [HttpDelete("{id}")]
        public IActionResult Inativar(int empresaId, int id)
        {
            try { _contaService.Inativar(id); return NoContent(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao inativar conta."); return BadRequest(e.Message); }
        }
    }

    [ApiController]
    [Route("api/empresas/{empresaId}/categorias-financeiras")]
    public class CategoriaFinanceiraController : ControllerBase
    {
        private readonly ILogger<CategoriaFinanceiraController> _logger;
        private readonly ICategoriaFinanceiraService _categoriaService;

        public CategoriaFinanceiraController(ILogger<CategoriaFinanceiraController> logger, ICategoriaFinanceiraService categoriaService)
        {
            _logger = logger;
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public IActionResult Listar(int empresaId)
        {
            try { return Ok(_categoriaService.Listar(empresaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar categorias."); return BadRequest(e.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int empresaId, int id)
        {
            try { return Ok(_categoriaService.Obter(empresaId, id)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter categoria."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public IActionResult Criar(int empresaId, [FromBody] CategoriaFinanceiraSalvarModel model)
        {
            model.IdEmpresa = empresaId;
            try { return Ok(_categoriaService.Criar(model)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar categoria."); return BadRequest(e.Message); }
        }

        [HttpPut("{id}")]
        public IActionResult Alterar(int empresaId, int id, [FromBody] CategoriaFinanceiraSalvarModel model)
        {
            model.IdEmpresa = empresaId;
            try { return Ok(_categoriaService.Alterar(id, model)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar categoria."); return BadRequest(e.Message); }
        }

        [HttpDelete("{id}")]
        public IActionResult Inativar(int empresaId, int id)
        {
            try { _categoriaService.Inativar(id); return NoContent(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao inativar categoria."); return BadRequest(e.Message); }
        }
    }

    [ApiController]
    [Route("api/empresas/{empresaId}/lancamentos-bancarios")]
    public class LancamentoBancarioController : ControllerBase
    {
        private readonly ILogger<LancamentoBancarioController> _logger;
        private readonly ILancamentoBancarioService _lancamentoService;

        public LancamentoBancarioController(ILogger<LancamentoBancarioController> logger, ILancamentoBancarioService lancamentoService)
        {
            _logger = logger;
            _lancamentoService = lancamentoService;
        }

        [HttpGet]
        public IActionResult Listar(int empresaId, [FromQuery] int contaId,
            [FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim)
        {
            var inicio = dataInicio ?? DateTime.Today.AddMonths(-1);
            var fim = dataFim ?? DateTime.Today;
            try { return Ok(_lancamentoService.Listar(empresaId, contaId, inicio, fim)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar lançamentos."); return BadRequest(e.Message); }
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int empresaId, int id)
        {
            try { return Ok(_lancamentoService.Obter(empresaId, id)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter lançamento."); return BadRequest(e.Message); }
        }

        [HttpPost]
        public IActionResult Criar(int empresaId, [FromBody] LancamentoBancarioSalvarModel model)
        {
            model.IdEmpresa = empresaId;
            try { return Ok(_lancamentoService.Criar(model)); }
            catch (ArgumentException e) { return BadRequest(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao criar lançamento."); return BadRequest(e.Message); }
        }

        [HttpPut("{id}")]
        public IActionResult Alterar(int empresaId, int id, [FromBody] LancamentoBancarioSalvarModel model)
        {
            model.IdEmpresa = empresaId;
            try { return Ok(_lancamentoService.Alterar(id, model)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao alterar lançamento."); return BadRequest(e.Message); }
        }

        [HttpDelete("{id}")]
        public IActionResult Cancelar(int empresaId, int id, [FromQuery] int idResponsavel)
        {
            try { _lancamentoService.Cancelar(id, idResponsavel); return NoContent(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao cancelar lançamento."); return BadRequest(e.Message); }
        }
    }

    [ApiController]
    [Route("api/empresas/{empresaId}/conciliacao-bancaria")]
    public class ConciliacaoBancariaController : ControllerBase
    {
        private readonly ILogger<ConciliacaoBancariaController> _logger;
        private readonly IConciliacaoBancariaService _conciliacaoService;

        public ConciliacaoBancariaController(ILogger<ConciliacaoBancariaController> logger, IConciliacaoBancariaService conciliacaoService)
        {
            _logger = logger;
            _conciliacaoService = conciliacaoService;
        }

        [HttpGet("pendentes")]
        public IActionResult ListarPendentes(int empresaId, [FromQuery] int? contaId)
        {
            try { return Ok(_conciliacaoService.ListarPendentes(empresaId, contaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar pendentes."); return BadRequest(e.Message); }
        }

        [HttpGet("sugestoes")]
        public IActionResult SugerirConciliacao(int empresaId, [FromQuery] int contaId)
        {
            try { return Ok(_conciliacaoService.SugerirConciliacao(empresaId, contaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao sugerir conciliação."); return BadRequest(e.Message); }
        }

        [HttpPost("conciliar")]
        public IActionResult Conciliar(int empresaId, [FromBody] ConciliarModel model)
        {
            model.IdEmpresa = empresaId;
            try { return Ok(_conciliacaoService.Conciliar(model)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao conciliar."); return BadRequest(e.Message); }
        }

        [HttpPost("desconciliar")]
        public IActionResult Desconciliar(int empresaId, [FromBody] DesconciliarModel model)
        {
            try { _conciliacaoService.Desconciliar(model); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao desconciliar."); return BadRequest(e.Message); }
        }
    }

    [ApiController]
    [Route("api/empresas/{empresaId}/fechamentos-bancarios")]
    public class FechamentoBancarioController : ControllerBase
    {
        private readonly ILogger<FechamentoBancarioController> _logger;
        private readonly IFechamentoBancarioService _fechamentoService;

        public FechamentoBancarioController(ILogger<FechamentoBancarioController> logger, IFechamentoBancarioService fechamentoService)
        {
            _logger = logger;
            _fechamentoService = fechamentoService;
        }

        [HttpGet]
        public IActionResult Listar(int empresaId)
        {
            try { return Ok(_fechamentoService.Listar(empresaId)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao listar fechamentos."); return BadRequest(e.Message); }
        }

        [HttpGet("{contaId}/{ano}/{mes}")]
        public IActionResult Obter(int empresaId, int contaId, int ano, int mes)
        {
            try { return Ok(_fechamentoService.Obter(empresaId, contaId, ano, mes)); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter fechamento."); return BadRequest(e.Message); }
        }

        [HttpPost("fechar")]
        public IActionResult Fechar(int empresaId, [FromBody] FecharMesModel model)
        {
            model.IdEmpresa = empresaId;
            try { return Ok(_fechamentoService.Fechar(model)); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao fechar mês."); return BadRequest(e.Message); }
        }

        [HttpPut("{id}/reabrir")]
        public IActionResult Reabrir(int empresaId, int id, [FromQuery] int idResponsavel)
        {
            try { _fechamentoService.Reabrir(id, idResponsavel); return Ok(); }
            catch (KeyNotFoundException e) { return NotFound(e.Message); }
            catch (InvalidOperationException e) { return BadRequest(e.Message); }
            catch (Exception e) { _logger.LogError(e, "Erro ao reabrir fechamento."); return BadRequest(e.Message); }
        }
    }

    [ApiController]
    [Route("api/empresas/{empresaId}/dashboard-bancario")]
    public class DashboardBancarioController : ControllerBase
    {
        private readonly ILogger<DashboardBancarioController> _logger;
        private readonly IDashboardBancarioService _dashboardService;

        public DashboardBancarioController(ILogger<DashboardBancarioController> logger, IDashboardBancarioService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public IActionResult ObterDashboard(int empresaId, [FromQuery] int? ano, [FromQuery] int? mes)
        {
            var anoParam = ano ?? DateTime.Today.Year;
            var mesParam = mes ?? DateTime.Today.Month;
            try { return Ok(_dashboardService.ObterDashboard(empresaId, anoParam, mesParam)); }
            catch (Exception e) { _logger.LogError(e, "Erro ao obter dashboard."); return BadRequest(e.Message); }
        }
    }
}
