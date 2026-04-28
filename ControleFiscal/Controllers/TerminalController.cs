using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using ControleFiscal.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics.Metrics;
using System.Text;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TerminalController : ControllerBase
    {

        private readonly ILogger<TerminalController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public TerminalController(ILogger<TerminalController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }


        [HttpGet("Reforma")]
        public ActionResult<List<Empresas>> Reforma()
        {
            try
            {
                var lista = _ContextLocal.Lojas.Where(x => x.Id < 200).ToList();
                //retorno com nome da loja e se tem o campo reforma
                var retorno = new List<object>();

                foreach (var loja in lista)
                {
                    _Context.ConexaoCliente(loja?.Caminho, loja?.Host);
                    var terminais = _Context.Terminais.ToList();
                    try
                    {
                        var atualizado = _Context.Pedido.Any();
                        retorno.Add(new { Loja = loja?.Nome ,TemCampoReforma = atualizado});
                    }
                    catch (Exception)
                    { 
                        retorno.Add(new { Loja = loja?.Nome,   TemCampoReforma = false });
                    }

                }
                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + e.StackTrace);
            }
        }


        [HttpPost("AtualizaNCM")]
        public ActionResult<List<RetornoAtualizacaoNCM>> AtualizarNCM()
        {
            var retorno = new List<RetornoAtualizacaoNCM>();

            var ncmConfiguracao = _ContextLocal.NCMs.FirstOrDefault(x => x.Padrao == "V");

            var ncm = ncmConfiguracao != null ? ncmConfiguracao?.NCM : string.Empty;

            if (ncm == string.Empty)
            {
                return BadRequest("NCM Padrão não encontrado");
            }

            try
            {
                var lojas = _ContextLocal.Lojas.Where(x => x.Id < 200).ToList();

                foreach (var loja in lojas)
                {
                    try
                    {
                        var conexaoLocal = new ContextControleFiscalContext();

                        conexaoLocal.ConexaoCliente(loja?.Caminho, loja?.Host);


                        var terminais = conexaoLocal.NfceConfigLocal.ToList();

                        foreach (var item in terminais)
                        {
                            item.EmissaoExtraordinariaNcm = ncm;
                            conexaoLocal.Update(item);
                            conexaoLocal.SaveChanges();

                        }
                        conexaoLocal.Dispose();
                        retorno.Add(new RetornoAtualizacaoNCM { Loja = loja?.Nome, Atualizado = true });
                    }
                    catch (Exception)
                    {
                        retorno.Add(new RetornoAtualizacaoNCM { Loja = loja?.Nome, Atualizado = false });                        
                    }
                }
                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + e.StackTrace);
            }
        }

        [HttpPost("AtualizarTipoCampoNCM")]
        public ActionResult AtualizarTipoCampo()
        {
            //precisa conectar em todas as lojas como feito na funcao acima e rodar esse script

               var retorno = new List<RetornoAtualizacaoNCM>();
            try
                {
                var lojas = _ContextLocal.Lojas.Where(x => x.Id < 200).ToList();
                foreach (var loja in lojas)
                {
                    try
                    {
                        var conexaoLocal = new ContextControleFiscalContext();
                        conexaoLocal.ConexaoCliente(loja?.Caminho, loja?.Host);
                        conexaoLocal.Database.ExecuteSqlRaw("ALTER TABLE NFCE_CONFIG_LOCAL ALTER COLUMN EMISSAO_EXTRAORDINARIA_NCM TYPE DM_TEXTO19999");
                        conexaoLocal.Dispose();
                        retorno.Add(new RetornoAtualizacaoNCM { Loja = loja?.Nome, Atualizado = true });
                    }
                    catch (Exception)
                    {
                        retorno.Add(new RetornoAtualizacaoNCM { Loja = loja?.Nome, Atualizado = false });
                    }
                }
                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + e.StackTrace);
            }

        }


        public class RetornoAtualizacaoNCM
        {
            public string Loja { get; set; }
            public bool Atualizado { get; set; }
        }

        [HttpGet("Obter")]
        public ActionResult<List<Object>> Obter([FromQuery] int lojaId, [FromQuery] DateTime? data = null)
        {
            string? nomeLoja = "";
            try
            {
                var loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);
                nomeLoja = loja?.Nome;
                                
                _Context.ConexaoCliente(loja?.Caminho, loja?.Host);

                if (data == null)
                {
                    data = DateTime.Now;
                }

                var lista = _Context.Terminais.Where(x => x.Deletado != "V").OrderBy(x => x.Descricao).ToList();

                var datas = DateHelper.GetFirstAndLastDayOfMonth(data.GetValueOrDefault().Month, data.GetValueOrDefault().Year);
                var inicio = datas.FirstDay;
                var fim = datas.LastDay;

                var resultado = _Context.NfcePedido.Where(p => p.DtEmissao >= inicio && p.DtEmissao <= fim && (p.Status == "E" || p.Status == "V" ))
                                                   .Join(
                                                        _Context.NfcePedidoItem.Where(x => x.DtEmissao >= inicio && x.DtEmissao <= fim), // Filtra os itens
                                                        pedido => new { pedido.SerieNfce, pedido.NumNfce, pedido.NumTerminal },        // Chave do pedido
                                                        item => new { item.SerieNfce, item.NumNfce, item.NumTerminal },             // Chave do item
                                                        (pedido, item ) => new { pedido, item }                    // Resultado combinado
                                                    )
                                                    .GroupBy(x => new { x.item.NumTerminal, x.item.Cfop })         // Agrupa pelos campos desejados
                                                    .Select(g => new RetornoCFOPTerminal
                                                    {
                                                        cfop = g.Key.Cfop,
                                                        NumTerminal = g.Key.NumTerminal,
                                                        Valor = g.Sum(p => p.item.Vprod - p.item.Vdesc)           // Soma o valor ajustado
                                                    }).ToList();


                IList<string> listaStatus = new List<string>();
                listaStatus.Add("C");
                listaStatus.Add("X");
                listaStatus.Add("P");
                listaStatus.Add("P");

                var contigencias = _Context.NfcePedido.Count(p => listaStatus.Contains(p.Status));
                       


                //var valorFiscal = _Context.NfcePedido.Where(p => p.DtEmissao >= inicio && p.DtEmissao <= DateTime.Now && (p.Status == "E" || p.Status == "V" || p.Status == "C" ))
                //                                     .GroupBy(p => p.NumTerminal)
                //                                     .Select(g => new RetornoValorTerminalDTO
                //                                     {
                //                                       NumTerminal = g.Key,
                //                                       Valor = g.Sum(p => p.VlTotalMercadorias)
                //                                     }).ToList();

                var valorPedido = _Context.Pedido.Where(p => p.DtFechamento >= inicio && p.DtFechamento <=  fim && p.Cancelado != "V" )
                                                 .GroupBy(p => p.NumTerminalFechamento)
                                                 .Select(g => new RetornoValorTerminalDTO
                                                 {
                                                    NumTerminal = g.Key.GetValueOrDefault(),
                                                    Valor = g.Sum(p => p.TotalLiquido)
                                                 })
                                                        .ToList();

                var nfceConfigList = _Context.NfceConfigLocal.ToList();

                //var nfcePedidoItem = _Context.NfcePedidoItem.Where(x => x.DtEmissao > inicio )
                //                                            .GroupBy(x => new { x.NumTerminal, x.Cfop })
                //                                            .Select(g => new RetornoCFOPTerminal { 
                //                                                cfop = g.Key.Cfop, 
                //                                                NumTerminal = g.Key.NumTerminal, 
                //                                                Valor = g.Sum(p => p.Vprod - p.Vdesc) 
                //                                            }).ToList();

                List<RetornoTerminaisDTO> retorno = new List<RetornoTerminaisDTO>();

                foreach (var item in lista)
                {
                    retorno.Add(new RetornoTerminaisDTO
                    {
                        Descricao = item.Descricao,
                        NumTerminal = item.NumTerminal,
                        EmissaoExtraordinaria = nfceConfigList.FirstOrDefault(x => x.NumTerminal == item.NumTerminal)?.EmissaoExtraordinaria == "V",
                        EmissaoExtraordinariaicms = nfceConfigList.FirstOrDefault(x => x.NumTerminal == item.NumTerminal)?.EmissaoExtraordinariaIcms == "V",
                        EmissaoExtraordinariancm = nfceConfigList.FirstOrDefault(x => x.NumTerminal == item.NumTerminal)?.EmissaoExtraordinariaNcm,
                        ValorFiscal = resultado == null ? 0 : resultado.Where(x => x.NumTerminal == item.NumTerminal).Sum(x => x.Valor),
                        ValorVendas = valorPedido == null ? 0 : valorPedido.Where(x => x.NumTerminal == item.NumTerminal).Sum(x => x.Valor),
                        NCM = nfceConfigList.FirstOrDefault(x => x.NumTerminal == item.NumTerminal)?.EmissaoExtraordinariaNcm,
                        cfop = resultado?.Where(x => x.NumTerminal == item.NumTerminal)?.ToList()

                    });
                }
                var valorFiscalTotal = resultado?.Sum(x => x.Valor); // _Context.NfcePedido.Where(p => p.DtEmissao > inicio).Sum(p => p.VlTotalNfce);
                var valorPedidoTotal = valorPedido?.Sum(x => x.Valor);// _Context.Pedido.Where(p => p.DtFechamento > inicio).Sum(p => p.TotalLiquido);

                var cfop = resultado?.GroupBy(x => x.cfop).Select(r => new RetornoCFOP { cfop = r.Key, Valor = r.Sum(p => p.Valor) });

                return new List<object> { new { valorFiscalTotal = valorFiscalTotal, valorPedidoTotal = valorPedidoTotal, cfop = cfop, Lista = retorno, Contingencia = contigencias } };
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }


        private ActionResult AtivarFiscalMargem(int lojaId = 0)
        {
            if (lojaId > 199)
            {
                return Unauthorized("Código da empresa não permite esta funcionalidade");
            }

            string? nomeLoja = "";
            try
            {
                List<Lojas> lojas = new();
                StringBuilder listErro = new();

                var NCMConfiguracao = _ContextLocal.NCMs.FirstOrDefault(x => x.Padrao == "V");

                var ncmPadrao = NCMConfiguracao?.NCM;


                lojas = _ContextLocal.Lojas.Where(x => lojaId == 0 ? true : x.Id == lojaId && x.Id < 200 && x.PercentualST > 0).ToList();

                foreach (var loja in lojas)
                {
                    try
                    {
                        nomeLoja = loja?.Nome;

                        ContextControleFiscalContext contextLoja = new ContextControleFiscalContext();

                        contextLoja.ConexaoCliente(loja?.Caminho, loja?.Host);

                        var lista = contextLoja.Terminais.Where(x => x.Deletado != "V").OrderBy(x => x.Descricao).OrderBy(x => x.NumTerminal).ToList();

                        DateTime inicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

                        var valorFiscal = contextLoja.NfcePedido.Where(p => p.DtEmissao > inicio && p.DtEmissao <= DateTime.Now)
                                                                 .GroupBy(p => p.NumTerminal)
                                                                 .Select(g => new RetornoValorTerminalDTO
                                                                 {
                                                                     NumTerminal = g.Key,
                                                                     Valor = g.Sum(p => p.VlTotalMercadorias)
                                                                 }).ToList();

                        var nfceConfigList = contextLoja.NfceConfigLocal.ToList();

                        var fiscal = contextLoja.NfcePedidoItem.Where(x => x.DtEmissao > inicio)
                                                                    .GroupBy(x => new { x.NumTerminal, x.Cfop })
                                                                    .Select(g => new RetornoCFOPTerminal
                                                                    {
                                                                        cfop = g.Key.Cfop,
                                                                        NumTerminal = g.Key.NumTerminal,
                                                                        Valor = g.Sum(p => p.Vprod)
                                                                    }).ToList();
                        foreach (var item in nfceConfigList)
                        {
                            var CFOP5405 = fiscal.Where(x => x.NumTerminal == item.NumTerminal && (x.cfop == "5.405" || x.cfop == "5405")).Sum(x => (double?)x.Valor) ?? 0; //ST
                            var CFOP5102 = fiscal.Where(x => x.NumTerminal == item.NumTerminal && (x.cfop == "5.102" || x.cfop == "5102")).Sum(x => (double?)x.Valor) ?? 0;

                            bool Habilitar = CFOP5405 > 0 && CFOP5102 >= (CFOP5405 * (loja?.PercentualST/100) );

                            item.EmissaoExtraordinariaIcms = Habilitar ? "V" : "F";
                            if (item.EmissaoExtraordinariaNcm == string.Empty)
                                item.EmissaoExtraordinariaNcm = ncmPadrao;

                            contextLoja.Update(item);
                            contextLoja.SaveChanges();

                        }

                    }
                    catch (Exception e)
                    {
                        listErro.Append(nomeLoja + "||" +  e.Message + "||" + e.StackTrace);
                    }

                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message + e.StackTrace);
            }

        }


        [HttpPost("MargemFiscal")]
        public ActionResult MargemFiscal([FromQuery] int lojaId)
        {
            if (lojaId > 199)
            {
                return Unauthorized("Código da empresa não permite esta funcionalidade");
            }

            return AtivarFiscalMargem(lojaId);
        }


        [HttpPost("AtualizarPercentualST")]
        public IActionResult AtualizarPercentualST([FromQuery] int lojaId, [FromQuery] double percentual)
        {
            return AtualizaPercentualST(lojaId,percentual);
        }

        private IActionResult AtualizaPercentualST(int lojaId , double percentual)        
        {
            if (lojaId > 199)
            {
                return Unauthorized("Código da empresa não permite esta funcionalidade"); 
            }
            var  loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);

            if (loja != null)
            {
                loja.PercentualST = percentual;
                _ContextLocal.Lojas.Update(loja);
                _ContextLocal.SaveChanges();

                return Ok();
            }
            return NotFound("Loja não encontrada");
        }

        [HttpPost("AtivarRecursoExtraordinario")]
        public ActionResult<bool> AtivarRecursoExtraordinario([FromQuery] int numTerminal, [FromQuery] int lojaId)
        {
            if (lojaId > 199)
            {
                return Unauthorized("Código da empresa não permite esta funcionalidade");
            }

            string? nomeLoja = "";
            try
            {
                var loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);
                nomeLoja = loja?.Nome;

                _Context.ConexaoCliente(loja?.Caminho, loja?.Host);

                var configLocal = _Context.NfceConfigLocal.FirstOrDefault(x => x.NumTerminal == numTerminal);

                if (configLocal != null)
                {
                    configLocal.EmissaoExtraordinaria = "V";

                    _Context.Update(configLocal);
                    _Context.SaveChanges();

                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }

        [HttpPost("DesativarRecursoExtraordinario")]
        public ActionResult<bool> DesativarRecursoExtraordinario([FromQuery] int numTerminal, [FromQuery] int lojaId)
        {

            string? nomeLoja = "";
            try
            {
                var loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);
                nomeLoja = loja?.Nome;

                _Context.ConexaoCliente(loja?.Caminho, loja?.Host);

                var configLocal = _Context.NfceConfigLocal.FirstOrDefault(x => x.NumTerminal == numTerminal);

                if (configLocal != null)
                {
                    configLocal.EmissaoExtraordinaria = "F";

                    _Context.Update(configLocal);
                    _Context.SaveChanges();

                    return true;
                }


                return false;
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }


        [HttpPost("AtivarRecursoExtraordinarioicms")]
        public ActionResult<bool> AtivarRecursoExtraordinarioicms([FromQuery] int numTerminal, [FromQuery] int lojaId, [FromQuery] string ncm)
        {

            if (lojaId > 199)
            {
                return Unauthorized("Código da empresa não permite esta funcionalidade");
            }

            string? nomeLoja = "";
            try
            {
                var loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);
                nomeLoja = loja?.Nome;

                _Context.ConexaoCliente(loja?.Caminho, loja?.Host);

                var configLocal = _Context.NfceConfigLocal.FirstOrDefault(x => x.NumTerminal == numTerminal);

                if (configLocal != null)
                {
                    configLocal.EmissaoExtraordinariaIcms = "V";
                    configLocal.EmissaoExtraordinariaNcm = ncm;

                    _Context.Update(configLocal);
                    _Context.SaveChanges();

                    return true;
                }
                 
                return false;
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }

        [HttpPost("DesativarRecursoExtraordinarioicms")]
        public ActionResult<bool> DesativarRecursoExtraordinarioicms([FromQuery] int numTerminal, [FromQuery] int lojaId)
        {
            string? nomeLoja = "";

            try
            {
                var loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);
                nomeLoja = loja?.Nome;

                _Context.ConexaoCliente(loja?.Caminho, loja?.Host);

                var configLocal = _Context.NfceConfigLocal.FirstOrDefault(x => x.NumTerminal == numTerminal);

               if (configLocal != null)
                {
                    configLocal.EmissaoExtraordinariaIcms = "F";

                    _Context.Update(configLocal);
                    _Context.SaveChanges();

                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }
    }
}