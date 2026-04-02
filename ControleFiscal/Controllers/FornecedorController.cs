using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Utils;
using Microsoft.AspNetCore.Mvc;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FornecedorController : ControllerBase
    {

        private readonly ILogger<FornecedorController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public FornecedorController(ILogger<FornecedorController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet("Obter")]
        public ActionResult<List<ComboDTO>> Obter([FromQuery] int lojaId)
        {
            var nomeLoja = "";
            try
            {
                var loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);
                nomeLoja = loja?.Nome;

                _Context.ConexaoCliente(loja?.Caminho, loja?.Host);

                var lista = _ContextLocal.Fornecedores.Where(x => x.idLoja == lojaId).ToList()
                                                      .OrderBy(x => x.NmFornecedor)
                                                      .Select(x => new ComboDTO { 
                                                          Id = x.CdFornecedor, 
                                                          Descricao = x.NmFornecedor 
                                                      });

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeLoja));
            }

        }

        [HttpPost("Sincronizar")]
        public IActionResult Sincronizar()
        {
            var retorno = new List<Dictionary<string,string>>();

            try
            {
                var lojas = _ContextLocal.Lojas.Where(x => x.Id < 999).ToList();

                foreach (var item in lojas)
                {
                    try
                    {
                        using (var  contextoLoja = new ContextControleFiscalContext())
                        {

                            var nomeLoja = contextoLoja.ConexaoCliente(item.Id, _ContextLocal);
                            var conectado = nomeLoja != string.Empty;

                            if (conectado)
                            {
                                var listaFornecedorLocal = _ContextLocal.Fornecedores.Where(x => x.idLoja == item.Id).ToList();

                                var listaFornecedorLoja = contextoLoja.Fornecedores.Where(x => x.Deletado != "V" && x.NmFornecedor != string.Empty)
                                                                                   .Select(x => new  Fornecedores()
                                                                                   {
                                                                                       CdFornecedor = x.CdFornecedor,
                                                                                       NmFornecedor = x.NmFornecedor
                                                                                   }).ToList();

                                foreach (var fornecLoja in listaFornecedorLoja)
                                {
                                    if (!listaFornecedorLocal.Any(x => x.CdFornecedor == fornecLoja.CdFornecedor && x.idLoja == item.Id))
                                    {
                                        var novo = new Fornecedor()
                                        {
                                            CdFornecedor = fornecLoja.CdFornecedor,
                                            idLoja = item.Id,
                                            NmFornecedor = fornecLoja.NmFornecedor
                                        };

                                        _ContextLocal.Fornecedores.Add(novo);
                                    }
                                    else
                                    {
                                        if (listaFornecedorLocal.Any(x => x.CdFornecedor == fornecLoja.CdFornecedor && x.NmFornecedor != fornecLoja.NmFornecedor && x.idLoja == item.Id))
                                        {
                                            var atualizar = _ContextLocal.Fornecedores.FirstOrDefault(x => x.CdFornecedor == fornecLoja.CdFornecedor && x.idLoja == item.Id);

                                            if (atualizar != null)
                                            {
                                                var fornecedorLocal = listaFornecedorLocal.FirstOrDefault(x => x.CdFornecedor == fornecLoja.CdFornecedor && x.idLoja == item.Id);
                                                atualizar.NmFornecedor = fornecedorLocal?.NmFornecedor;
                                                _ContextLocal.Fornecedores.Update(atualizar);
                                            }

                                        }
                                    }


                                }
                                _ContextLocal.SaveChanges();
                                listaFornecedorLocal.Clear();
                                listaFornecedorLoja.Clear();
                                retorno.Add( new Dictionary<string, string> {{ item.Nome, "true" } });
                            }
                        } 
                    }
                    catch (Exception e)
                    {
                        retorno.Add(new Dictionary<string, string> { { item.Nome, e.Message } });
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            return Ok(retorno);
        }
    }
}