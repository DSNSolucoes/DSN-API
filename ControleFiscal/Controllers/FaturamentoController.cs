using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Utils;

using ControleFiscal.Infrastructure.Sql.Focus;
using Microsoft.AspNetCore.Mvc;
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaturamentoController : ControllerBase
    {

        private readonly ILogger<FaturamentoController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public FaturamentoController(ILogger<FaturamentoController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet("Obter")]
        public IActionResult Obter( [FromQuery] DateTime? data = null)
        {
            string? nomeLoja = "";
            try
            {
                var loja = _ContextLocal.Empresas.Where(x => x.Id < 900);

                List<RetornoResultadoEmpresasDTO> retorno = new List<RetornoResultadoEmpresasDTO>();

                foreach (var item in loja) {
                    try
                    {
                        nomeLoja = item?.Nome;
                        
            
                        using (ContextControleFiscalContext contextAtual = new ContextControleFiscalContext())
                        {
                            contextAtual.ConexaoCliente(item?.Caminho ?? "", item?.Host ?? "");

                            if (data == null)
                            {
                                data = DateTime.Now;
                            }

                            var lista = contextAtual.Terminais.Where(x => x.Deletado != "V").OrderBy(x => x.Descricao).ToList();

                            var datas = DateHelper.GetFirstAndLastDayOfMonth(data.GetValueOrDefault().Month, data.GetValueOrDefault().Year);
                            var inicio = datas.FirstDay;
                            var fim = datas.LastDay;

                            var resultado =  contextAtual.NfcePedido.Where(p => p.DtEmissao >= inicio && p.DtEmissao <= fim && (p.Status == "E" || p.Status == "V"))
                                                               .Join(
                                                                    contextAtual.NfcePedidoItem.Where(x => x.DtEmissao >= inicio && x.DtEmissao <= fim), // Filtra os itens
                                                                    pedido => new { pedido.SerieNfce, pedido.NumNfce, pedido.NumTerminal },        // Chave do pedido
                                                                    item => new { item.SerieNfce, item.NumNfce, item.NumTerminal },             // Chave do item
                                                                    (pedido, item) => new { pedido, item }                    // Resultado combinado
                                                                )
                                                                .GroupBy(x => new { x.item.NumTerminal, x.item.Cfop })         // Agrupa pelos campos desejados
                                                                .Select(g => new RetornoCFOPTerminal
                                                                {
                                                                    cfop = g.Key.Cfop,
                                                                    NumTerminal = g.Key.NumTerminal,
                                                                    Valor = g.Sum(p => p.item.Vprod - p.item.Vdesc)           // Soma o valor ajustado
                                                                }).ToList();


                            var valorPedido =  contextAtual.Pedido.Where(p => p.DtFechamento >= inicio && p.DtFechamento <= fim && p.Cancelado != "V")
                                                             .GroupBy(p => p.NumTerminalFechamento)
                                                             .Select(g => new RetornoValorTerminalDTO
                                                             {
                                                                 NumTerminal = g.Key.GetValueOrDefault(),
                                                                 Valor = g.Sum(p => p.TotalLiquido)
                                                             })
                                                                    .ToList();



                            retorno.Add(new RetornoResultadoEmpresasDTO
                            {
                                loja = nomeLoja,
                                ValorFiscal = resultado == null ? 0 : resultado.Sum(x => x.Valor),
                                ValorVendas = valorPedido == null ? 0 : valorPedido.Sum(x => x.Valor),
                                CFOP = resultado?.GroupBy(x => x.cfop).Select(r => new RetornoCFOP { cfop = r.Key, Valor = r.Sum(p => p.Valor) }).ToList()

                            });
                        }                        
                         
                    }
                    catch (Exception) 
                    {

                    }

                }

                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }


    }
}