using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Utils;
using Microsoft.AspNetCore.Mvc;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql.Focus.Context;



namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NFCeController : ControllerBase
    {

        private readonly ILogger<NFCeController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public NFCeController(ILogger<NFCeController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet("ObterEnviadas")]
        public ActionResult<List<NfcePedido>> ObterEnviadas([FromQuery] int lojaId, [FromQuery] bool somenteExtraordinaria)
        {
            var nomeLoja = "";
            try
            {
                nomeLoja = _Context.ConexaoCliente(lojaId, _ContextLocal);

                List<NfcePedido> lista = new List<NfcePedido>();

                if (somenteExtraordinaria)
                {
                    lista = _Context.NfcePedido.Where(x => x.Extraordinaria == "V").Take(100).ToList();
                }
                else
                {
                    lista = _Context.NfcePedido.Where(x => x.Status == "E").Take(100).ToList();
                }

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }

        [HttpGet("ObterEnviadasCFOP")]
        public ActionResult<List<RetornoEmissaoCFOPDTO>> ObterEnviadasPorCFOP([FromQuery] int lojaId, [FromQuery] DateTime inicio, [FromQuery] DateTime fim)
        {
            var nomeLoja = "";
            try
            {
                var loja = _ContextLocal.Empresas.FirstOrDefault(x => x.Id == lojaId);
                nomeLoja = loja?.Nome;

                _Context.ConexaoCliente(loja!.Caminho, loja!.Host);

                List<RetornoEmissaoCFOPDTO> retorno = new List<RetornoEmissaoCFOPDTO>();

                var lista = _Context.NfcePedidoItem.
                                     Where(x => x.DtEmissao > inicio && x.DtEmissao < fim).
                                     Select(x => new RetornoEmissaoCFOPDTO
                                     {
                                         dtEmissao = x.DtEmissao,
                                         CFOP = x.Cfop,
                                         vlTotalNfce = x.Vprod.GetValueOrDefault()
                                     }
                                     ).ToList().GroupBy(p => p.CFOP);


                if (lista.Count() > 0)
                {
                    foreach (var item in lista)
                    {
                        if (item != null)
                        {
                            retorno.Add(new RetornoEmissaoCFOPDTO { CFOP = item.First().CFOP, dtEmissao = item.First().dtEmissao, vlTotalNfce = item.First().vlTotalNfce });
                        }
                    }
                }

                return retorno;
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }
        }



        [HttpGet("ObterContingencia")]
        public ActionResult<List<NfcePedido>> ObterContingencia([FromQuery] int lojaId,
                                                  [FromQuery] DateTime? filtroInicio,
                                                  [FromQuery] DateTime? filtroFim,
                                                  [FromQuery] int? terminalId)
        {

            string? nomeLoja = "";
            try
            {
                var loja = _ContextLocal.Empresas.FirstOrDefault(x => x.Id == lojaId);
                nomeLoja = loja?.Nome;

                _Context.ConexaoCliente(loja?.Caminho ?? "", loja?.Host ?? "");

                var lista = _Context.NfcePedido.Where(x => x.Status == "C").Take(100).ToList();


                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }
        }


    }
}