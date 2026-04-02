using ControleFiscal.Infrastructure.Sql.Focus; 
using ControleFiscal.Infrastructure.Sql; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Text;
using ControleFiscal.Domain.DTO.Relatorio;
using ControleFiscal.Infrastructure.Sql.Focus.Context;
namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatorioF04Controller : ControllerBase
    {

        private readonly ILogger<RelatorioF04Controller> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public RelatorioF04Controller(ILogger<RelatorioF04Controller> logger, ContextControleFiscalContext Context, ContextLocalContext ContextoLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextoLocal;
        }

        [HttpGet("ObterAgrupado")]
        public ActionResult<List<RelatorioF04DTOAgrupado>> Obter([FromQuery] List<int> lojaId, [FromQuery] int? fornecedorId, [FromQuery] int? grupoId, [FromQuery] string inicio, [FromQuery] string fim, [FromQuery] int ordenacao = 0, [FromQuery] string ascdesc = "ASC")
        {
            try
            {
                var retorno = ObterDadosAgrupado(lojaId, fornecedorId, grupoId, inicio, fim, ordenacao, ascdesc);
                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("ObterDetalhado")]
        public ActionResult<List<RelatorioF04DTO>> ObterDetalhado([FromQuery] List<int> lojaId, [FromQuery] int? fornecedorId, [FromQuery] int? grupoId, [FromQuery] string inicio, [FromQuery] string fim, [FromQuery] int ordenacao = 0, [FromQuery] string ascdesc = "ASC") 

        {
            try
            {
                var retorno = ObterDadosDetalhado(lojaId, fornecedorId,grupoId, inicio, fim, ordenacao, ascdesc);
                
                return Ok(retorno);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private List<RelatorioF04DTOAgrupado> ObterDadosAgrupado(List<int> lojaId, int? fornecedorId, int? grupoId, string inicio, string fim, int ordenacao = 0, string ascdesc = "ASC")
        {
            try
            {
                var nomeLoja = _Context.ConexaoCliente(lojaId.First(), _ContextLocal);

                StringBuilder sql = new StringBuilder();

                sql.Append("select ");
                sql.Append("F.CD_FORNECEDOR as FornecedorId, ");
                sql.Append("F.NM_FORNECEDOR as Fornecedor, ");
                sql.Append("I.UNIDADE as Unidade, ");
                sql.Append("sum(I.QUANTIDADE) as Quantidade, ");
                sql.Append("cast(sum(I.SUBTOTAL) as numeric(15, 2)) as SubTotal ");
                sql.Append("from fornecedores F ");
                sql.Append("join PRODFORNEC PF on PF.CD_FORNECEDOR = F.CD_FORNECEDOR ");
                sql.Append("join PRODUTOS P on P.CD_PRODUTO = PF.CD_PRODUTO ");
                sql.Append("join PEDIDO_ITEM I on I.CD_PRODUTO = P.CD_PRODUTO ");
                sql.Append("join PEDIDO V on V.NUM_DOCUMENTO = I.NUM_DOCUMENTO ");
                sql.Append("where ");
                sql.Append("V.DT_FECHAMENTO between {0} and {1} ");
                sql.Append("AND V.CANCELADO<> 'V' ");
                sql.Append("and V.TIPO_DOCUMENTO = 'V' ");
                sql.Append("and I.CANCELADO<> 'V' ");

                if (fornecedorId > 0)
                {
                    sql.Append("and coalesce(F.Deletado, 'F') <> 'V' ");
                    sql.Append("and F.CD_FORNECEDOR = {2} ");
                }

                sql.Append("group by 1, 2, 3");

                DateTime dataInicio = new DateTime(DateTime.Parse(inicio).Year, DateTime.Parse(inicio).Month, DateTime.Parse(inicio).Day, 0, 0, 1);
                DateTime dataFim = new DateTime(DateTime.Parse(fim).Year, DateTime.Parse(fim).Month, DateTime.Parse(fim).Day, 23, 59, 59);

                var retorno = _Context.RelatoriosF04Agrupado.FromSqlRaw(@sql.ToString(), dataInicio, dataFim,fornecedorId).ToList();

                ascdesc = ascdesc.ToUpper().Trim();

                switch (ordenacao)
                {   
                    case 1 :
                        {
                            return ascdesc == "ASC" ?   retorno.OrderBy(x => x.FornecedorId).ToList() : retorno = retorno.OrderByDescending(x => x.FornecedorId).ToList();                            
                        }
                    case 2 :
                        {
                            return ascdesc == "ASC" ?   retorno.OrderBy(x => x.Fornecedor).ToList() : retorno = retorno.OrderByDescending(x => x.Fornecedor).ToList();                            
                        }
                    case 3:
                        {
                            return ascdesc == "ASC" ?   retorno.OrderBy(x => x.Quantidade).ToList() : retorno = retorno.OrderByDescending(x => x.Quantidade).ToList();                            
                        }
                    case 4:
                        {
                            return ascdesc == "ASC" ?   retorno.OrderBy(x => x.SubTotal).ToList() : retorno = retorno.OrderByDescending(x => x.SubTotal).ToList();                            
                        }
                    default:
                        return retorno.OrderByDescending(x => x.SubTotal).ToList();  
                         
                } 

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao obter dados agrupados: " + e.Message);
            }


        }

        private List<RelatorioF04DTO> ObterDadosDetalhado(List<int> lojaId, int? fornecedorId, int? grupoId, string inicio , string fim, int ordenacao = 0, string ascdesc = "ASC")
        {
            try
            {

                var nomeLoja = _Context.ConexaoCliente(lojaId.First(), _ContextLocal);

                StringBuilder sql = new StringBuilder();
               
                {
                    sql.Append("SELECT ");
                    sql.Append("P.COD_BARRAS as CODIGOBARRAS, ");
                    sql.Append("COALESCE(I.NM_PRODUTO, P.NM_PRODUTO) as NOMEPRODUTO, ");
                    sql.Append("I.UNIDADE as UNIDADE, ");
                    sql.Append("SUM(I.QUANTIDADE) as QUANTIDADE, ");
                    sql.Append("CAST(AVG(I.preco) as numeric(15,2)) as PRECO, ");
                    sql.Append("CAST(Sum(I.SUBTOTAL) as numeric(15,2)) as SUBTOTAL ");
                    sql.Append("FROM FORNECEDORES F ");
                    sql.Append("JOIN PRODFORNEC PF on PF.CD_FORNECEDOR = F.CD_FORNECEDOR ");
                    sql.Append("JOIN PRODUTOS P on P.CD_PRODUTO = PF.CD_PRODUTO ");
                    sql.Append("JOIN PEDIDO_ITEM I on I.CD_PRODUTO = P.CD_PRODUTO ");
                    sql.Append("JOIN PEDIDO V on V.NUM_DOCUMENTO = I.NUM_DOCUMENTO ");
                    sql.Append("WHERE ");
                    sql.Append("V.DT_FECHAMENTO between {0} and {1} ");
                    sql.Append("AND V.CANCELADO <> 'V' ");
                    sql.Append("and V.TIPO_DOCUMENTO = 'V' ");
                    sql.Append("and I.CANCELADO <> 'V' ");

                    if (grupoId > 0)
                    {
                        sql.Append("AND P.CD_GRUPO = {2}");
                    }

                    if (fornecedorId > 0)
                    {
                        sql.Append("AND COALESCE(F.DELETADO, 'F') <> 'V' ");
                        sql.Append("AND F.CD_FORNECEDOR = {3}");
                    }

                    sql.Append("GROUP BY 1,2,3");
                }


                DateTime dataInicio = new DateTime(DateTime.Parse(inicio).Year, DateTime.Parse(inicio).Month, DateTime.Parse(inicio).Day, 0, 0, 1);
                DateTime dataFim = new DateTime(DateTime.Parse(fim).Year, DateTime.Parse(fim).Month, DateTime.Parse(fim).Day, 23, 59, 59);

                var retorno = _Context.RelatoriosF04.FromSqlRaw(@sql.ToString(), dataInicio, dataFim,grupoId,fornecedorId).ToList();

                switch (ordenacao)
                {
                    case 1:
                        {
                            return ascdesc == "ASC" ? retorno.OrderBy(x => x.CodigoBarras).ToList() : retorno = retorno.OrderByDescending(x => x.CodigoBarras).ToList();                            
                        }
                    case 2:
                        {
                            return ascdesc == "ASC" ? retorno.OrderBy(x => x.NomeProduto).ToList() : retorno = retorno.OrderByDescending(x => x.NomeProduto).ToList();
                            
                        }
                    case 3:
                        {
                            return ascdesc == "ASC" ? retorno.OrderBy(x => x.Quantidade).ToList() : retorno = retorno.OrderByDescending(x => x.Quantidade).ToList();
                            
                        }
                    case 4:
                        {
                            return ascdesc == "ASC" ? retorno.OrderBy(x => x.SubTotal).ToList() : retorno = retorno.OrderByDescending(x => x.SubTotal).ToList();
                            
                        }
                    default:
                        return retorno.OrderByDescending(x => x.SubTotal).ToList();                         
                }
                  

            }
            catch (Exception e)
            {
                throw new Exception("Erro ao obter dados agrupados: " + e.Message);
            }


        }


        [HttpGet("exportar")]
        public IActionResult ExportarRelatorio([FromQuery] List<int> lojaId, [FromQuery] bool? agrupado, [FromQuery] int? modelo, [FromQuery] bool? emissao, [FromQuery] bool? bonificacao, [FromQuery] int? fornecedorId, [FromQuery] string inicio, [FromQuery] string fim, [FromQuery] int ordenacao = 0, [FromQuery] string ascdesc = "ASC") 
        {
            try
            {                 
                StringBuilder relatorio = new StringBuilder();             

                if (agrupado == true)
                {
                    var resultadoAgrupado = ObterDadosAgrupado(lojaId, fornecedorId, modelo, inicio, fim, ordenacao, ascdesc).ToList();
                    var lista = resultadoAgrupado.Cast<RelatorioF04DTOAgrupado>().ToList();

                    relatorio.AppendLine("Fornecedor Id;Fornecedor;Unidade;Quantidade;SubTotal");

                    foreach (var item in lista)
                    {
                        relatorio.Append($"\"{item.FornecedorId}\";");
                        relatorio.Append($"\"{item.Fornecedor}\";");
                        relatorio.Append($"\"{item.Unidade}\";");
                        relatorio.Append($"\"{item.Quantidade}\";");
                        relatorio.Append($"\"{item.SubTotal}\";");                        
                        relatorio.AppendLine("");
                    }

                }
                else
                {
                    var resultadoDetalhado = ObterDadosDetalhado(lojaId, fornecedorId, modelo, inicio, fim, ordenacao, ascdesc).ToList();
                    var lista = resultadoDetalhado.Cast<RelatorioF04DTO>().ToList();

                    relatorio.AppendLine("Cód. Barras;Produto;Unidade;Preço;Quantidade;SubTotal");
                     foreach (var item in lista)
                      {
                        relatorio.Append($"\"{item.CodigoBarras}\";");
                        relatorio.Append($"\"{item.NomeProduto}\";");
                        relatorio.Append($"\"{item.Unidade}\";");
                        relatorio.Append($"\"{item.Preco}\";");
                        relatorio.Append($"\"{item.Quantidade}\";");
                        relatorio.Append($"\"{item.SubTotal}\";");
                        relatorio.AppendLine("");
                    }
                }

                string nomeArquivo = $"Relatorio_F04_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(relatorio.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }





    }
}