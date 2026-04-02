using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Utils; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IntegracaoController : ControllerBase
    {

        private readonly ILogger<IntegracaoController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public IntegracaoController(ILogger<IntegracaoController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet("ObterVendaDiariaDetalhado")]
        [Authorize]
        public ActionResult<RetornoValorVendaDiariaDetalhado> VendaDiariaDetalhado([FromQuery] int empresaId, [FromQuery] int mes, [FromQuery] int ano)
        {
            return CarregarVendaDiariaDetalhadoPorEmpresa(IdentificaLoja(empresaId), mes, ano);

        }

        [HttpGet("ObterVendaMensalDetalhado")]
        [Authorize]
        public ActionResult<List<RetornoValorVendaMensalDetalhado>> VendaMensalDetalhado([FromQuery] int empresaId, [FromQuery] int mes, [FromQuery] int ano )
        { 
            return CarregarVendaMensalDetalhadoPorEmpresa(IdentificaLoja(empresaId),mes, ano);
        }     

        [HttpGet("ObterValorVendaMensal")]
        [Authorize]
        public ActionResult<RetornoValorVendaMensal> ObterMensal([FromQuery] int empresaId ,[FromQuery] int mes, [FromQuery] int ano)  
        { 
          return CarregarValorVendaMensal(IdentificaLoja(empresaId), mes, ano);
        }

        private ActionResult<RetornoValorVendaMensal> CarregarValorVendaMensal( int empresaId, int mes, int ano)
        { 
            if (empresaId <= 0)
            {
                return BadRequest(HttpStatusCode.Unauthorized);
            }

            try
            {
                var nomeEmpresa = _Context.ConexaoCliente(empresaId, _ContextLocal);
                try
                {
                    var (inicio, fim) = DateHelper.GetFirstAndLastDayOfMonth(mes, ano);

                    decimal valorVendaMensal = _Context.Pedido.Where(x => x.Cancelado != "V" && x.TipoDocumento == "V" && x.DtFechamento >= inicio && x.DtFechamento <= fim)
                                                              .Sum(x => x.TotalLiquido - x.PagCredcli).GetValueOrDefault();

                    return new RetornoValorVendaMensal
                    {
                        Mes = mes,
                        Ano = DateTime.Now.Year,
                        DataConsulta = DateTime.Now,
                        DataFinal = fim,
                        DataInicial = inicio,
                        Valor = valorVendaMensal,
                        ValorFormatado = ConverterHelper.FormatarRealBrasileiro(valorVendaMensal)
                    };

                }
                catch (Exception e)
                {
                    return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeEmpresa));
                }
            }
            catch (Exception )
            {
                return BadRequest("Falha na conexão com a loja.");
            }
        }

        private ActionResult<List<RetornoValorVendaMensalDetalhado>> CarregarVendaMensalDetalhadoPorEmpresa(int loja,int mes, int ano)
        {
            if (loja <= 0)
            {
                return BadRequest(HttpStatusCode.Unauthorized);
            }
            try
            {
                _logger.LogInformation("Iniciando conexão de banco de dados.");
                _Context.ConexaoCliente(loja, _ContextLocal);
                _logger.LogInformation("Conexão realizado com sucesso.");

                var (inicio, fim) = DateHelper.GetFirstAndLastDayOfMonth(mes, ano);

                _logger.LogInformation("Iniciando query");
                var vendaMensal = _Context.Pedido.Where(x => x.Cancelado != "V" && x.TipoDocumento == "V" && 
                                                             x.DtFechamento >= inicio && x.DtFechamento <= fim)
                                                      .GroupBy(x => x.DtFechamento.Date)
                                                      .Select(g => new { DtFechamento = g.Key, TotalLiquido = g.Sum(x => x.TotalLiquido - x.PagCredcli) }).ToList();

                _logger.LogInformation("Preenchendo retorno.");
                List<RetornoValorVendaMensalDetalhado> listaDetalhado = new List<RetornoValorVendaMensalDetalhado>();

                foreach (var item in vendaMensal)
                {
                    RetornoValorVendaMensalDetalhado itemAtual = new RetornoValorVendaMensalDetalhado();
                    itemAtual.Valor = item.TotalLiquido.GetValueOrDefault();
                    itemAtual.Dia = item.DtFechamento;
                    itemAtual.ValorFormatado = ConverterHelper.FormatarRealBrasileiro(item.TotalLiquido.GetValueOrDefault());

                    listaDetalhado.Add(itemAtual);
                }

                return listaDetalhado;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing VendaMensalDetalhado for month {Month}", mes);
                return BadRequest(e.Message);
            }
        }

        private ActionResult<RetornoValorVendaDiariaDetalhado> CarregarVendaDiariaDetalhadoPorEmpresa(int loja, int mes, int ano)
        {
            if (loja <= 0)
            {
                return BadRequest(HttpStatusCode.Unauthorized);
            }
            try
            {
                _logger.LogInformation("Iniciando conexão de banco de dados.");
                _Context.ConexaoCliente(loja, _ContextLocal);
                _logger.LogInformation("Conexão realizado com sucesso.");

                var (inicio, fim) = DateHelper.GetFirstAndLastDayOfMonth(mes, ano);

                _logger.LogInformation("Iniciando query");
                var vendaMensal = _Context.Pedido.Where(x => x.Cancelado != "V" && x.TipoDocumento == "V" &&
                                                             x.DtFechamento >= inicio && x.DtFechamento <= fim)
                                                      .GroupBy(x => new { x.DtFechamento, x.NumDocumento})
                                                      .Select(g => new { DtFechamento = g.Key.DtFechamento, TotalLiquido = g.Sum(x => x.TotalLiquido - x.PagCredcli), Identificador = g.Key.NumDocumento }).ToList();


                _logger.LogInformation("Preenchendo retorno.");
                List<RetornoValorVendaMensalDetalhado> listaDetalhado = new List<RetornoValorVendaMensalDetalhado>();
                var retorno = new RetornoValorVendaDiariaDetalhado();
                retorno.Dias = new List<RetornoValorVendaMensalDetalhado>();
                foreach (var item in vendaMensal)
                {
                    RetornoValorVendaMensalDetalhado itemAtual = new RetornoValorVendaMensalDetalhado();
                    itemAtual.Valor = item.TotalLiquido.GetValueOrDefault();
                    itemAtual.Dia = item.DtFechamento;
                    itemAtual.ValorFormatado = ConverterHelper.FormatarRealBrasileiro(item.TotalLiquido.GetValueOrDefault());
                    itemAtual.Identificador = item.Identificador;

                    listaDetalhado.Add(itemAtual);
                    retorno.Valor = retorno.Valor + itemAtual.Valor;
                    retorno.Dias.Add(itemAtual);
                }

                retorno.ValorFormatado = ConverterHelper.FormatarRealBrasileiro(retorno.Valor); 

                return retorno;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while processing VendaMensalDetalhado for month {Month}", mes);
                return BadRequest(e.Message);
            }
        }
        private static int IdentificaLoja(int idIntegracao)
        {
            switch (idIntegracao)
            {
                case 1302:
                    return 11; //NortShopping
                  

                case 1303:
                    return 17; //Carioca

                case 1304:
                    return 12; //Ilha
                    

                case 1305:
                    return 18; //Tijuca
                   

                case 1306:
                    return 22; //Macare
                  

                default:
                    return -1;
                    
            }
        }

        public class RetornoValorVendaMensal
        {
            public int Mes { get; set; }
            public int Ano { get; set; }
            public string? ValorFormatado { get; set; }
            public decimal Valor { get; set; }
            public DateTime DataConsulta { get; set; }
            public DateTime DataInicial { get; set; }
            public DateTime DataFinal { get; set; }
        }
        public class RetornoValorVendaMensalDetalhado
        {
            public int? Identificador { get; set; }
            public DateTime Dia { get; set; }            
            public string? ValorFormatado { get; set; }
            public decimal Valor { get; set; }
        }
        public class RetornoValorVendaDiariaDetalhado
        {
            public List<RetornoValorVendaMensalDetalhado>? Dias { get; set; }            
            public string? ValorFormatado { get; set; }
            public decimal Valor { get; set; }

        }
    }
}