using ControleFiscal.Context.NFe;
using ControleFiscal.Infrastructure.Sql;

using Microsoft.AspNetCore.Mvc; 
using System.Xml.Serialization;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NFeController : ControllerBase
    {
        private readonly ILogger<NFeController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public NFeController(ILogger<NFeController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }


        [HttpPost("ObterDados")]
        public IActionResult ObterEnviadas([FromQuery] List<IFormFile> Arquivos, [FromQuery] string CNPJ)
        {
            NFeSerialization serializable = new NFeSerialization();
            List<NFeProc> listaNFe = new List<NFeProc>(); 
            var serializer = new XmlSerializer(typeof(NFeProc));

            foreach (var file in Arquivos)
            {
                if (file.Length > 0)
                {
                    var filePath = "C:\\TEMP\\" + CNPJ ;

                    if (!Directory.Exists(filePath))
                      Directory.CreateDirectory(filePath);

                    var filePathName = filePath + file.FileName;

                    if (!System.IO.File.Exists(filePathName))
                      System.IO.File.Delete(filePathName);

                     
                    using (var arquivo = new MemoryStream())
                    {
                        file.OpenReadStream().CopyTo(arquivo);
                        arquivo.Position = 0;
                        var nfe = serializable.GetObjectFromFile<NFeProc>(arquivo);
                        if (nfe != null)
                        {
                            listaNFe.Add(nfe);
                        }
                    }
                     
                }
            }

            var total = listaNFe.Count;
            RetornoNFe retorno = new RetornoNFe();

            foreach (var nota in listaNFe)
            {
                if (nota.NotaFiscalEletronica?.InformacoesNFe?.Detalhe?.Count() > 0)
                {               
                    foreach (var item in nota.NotaFiscalEletronica.InformacoesNFe.Detalhe)
                    {
                        if  (retorno?.CFOPs?.Count() > 0 && retorno.CFOPs.Any(x => x.CFOP == item.Produto.CFOP))
                        {
                            var ret = retorno?.CFOPs?.FirstOrDefault(x => x.CFOP == item.Produto.CFOP);

                            if (ret != null)                                
                              ret.Valor = item.Produto.ValorBrutoProdutoServico;
                        }
                        else
                        {
                             retorno?.CFOPs?.Add(new CFOPValores { CFOP = item.Produto.CFOP, Valor = item.Produto.ValorBrutoProdutoServico });
                        }                    

                    }

                }
            }


            return Ok(retorno);

        }



    }

    public class RetornoNFe 
    {
        public List<CFOPValores>? CFOPs { get; set; }
    }

    public class CFOPValores
    {
        public string? CFOP { get; set; }
        public decimal Valor { get; set; }
    }

}
