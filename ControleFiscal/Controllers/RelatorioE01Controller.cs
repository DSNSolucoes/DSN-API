using ControleFiscal.Infrastructure.Sql; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using ControleFiscal.Domain.DTO.Relatorio;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatorioE01Controller : ControllerBase
    {

        private readonly ILogger<RelatorioE01Controller> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public RelatorioE01Controller(ILogger<RelatorioE01Controller> logger, ContextControleFiscalContext Context, ContextLocalContext ContextoLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextoLocal;
        }

        [HttpGet]
        public ActionResult<List<RelatorioE01DTO>> Obter([FromQuery] List<int> lojaId,[FromQuery] bool? agrupado,  [FromQuery] int? modelo,[FromQuery] bool? emissao,  [FromQuery] bool? bonificacao,  [FromQuery] int? fornecedorId, [FromQuery] string inicio, [FromQuery] string fim, [FromQuery] int ordenacao = 0, [FromQuery] string ascdesc = "ASC")
        {
            try
            {
                var retorno = ObterDados(lojaId, agrupado, modelo, emissao, bonificacao, fornecedorId, inicio, fim, ordenacao, ascdesc);
                return Ok(retorno);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }



        private List<RelatorioE01DTO> ObterDados(List<int> lojaId, bool? agrupado, int? modelo, bool? emissao, bool? bonificacao, int? fornecedorId, string inicio, string fim, int ordenacao = 0, string? ascdesc = "ASC")
        {
            try
            {
                var nomeLoja = _Context.ConexaoCliente(lojaId.First(), _ContextLocal);

                StringBuilder sql = new StringBuilder();
                //Agruapado

                sql.Append("select distinct ");
                sql.Append(" ENTRADA.DT_EMISSAO AS EMISSAO, ");
                sql.Append(" ENTRADA.DT_CADASTRO AS CADASTRO, ");
                sql.Append(" ENTRADA.DT_ENTRADA AS ENTRADA, ");
                sql.Append(" ENTRADA.CD_FORNECEDOR AS  FORNECEDORID, ");
                sql.Append(" FORNECEDORES.NM_FORNECEDOR   AS FORNECEDOR, ");
                sql.Append(" ENTRADA.NUM_DOCUMENTO  AS NF, ");
                sql.Append(" cast(ENTRADA.VR_TOTAL_PROD as numeric(15,2)) VALORPRODUTO, ");
                sql.Append(" cast(ENTRADA.VALOR_TOTAL_NOTA as numeric(15,2)) AS VALORTOTAL, ");



                if (agrupado == true)
                {
                    sql.Append(" 0 as PARCELA, ");
                    sql.Append(" 0 as VALORPARCELA, ");
                    sql.Append(" current_timestamp as VENCIMENTO ");
                    sql.Append(" from ENTRADA ");
                    sql.Append(" left outer JOIN FORNECEDORES ");
                    sql.Append(" on ENTRADA.CD_FORNECEDOR = FORNECEDORES.CD_FORNECEDOR");
                    sql.Append(" WHERE");
                }
                else
                {
                    sql.Append(" coalesce (contasapagar.parcela, 1) as PARCELA, ");
                    sql.Append(" cast(coalesce (contasapagar.valor, ENTRADA.VALOR_TOTAL_NOTA) as numeric(15,2)) as VALORPARCELA, ");
                    sql.Append(" coalesce (contasapagar.vencimento, ENTRADA.DT_ENTRADA) as VENCIMENTO ");
                    sql.Append(" from ENTRADA ");
                    sql.Append(" left outer JOIN FORNECEDORES on ENTRADA.CD_FORNECEDOR = FORNECEDORES.CD_FORNECEDOR ");
                    sql.Append(" left outer join contasapagar on ENTRADA.cd_nota = contasapagar.cd_nota ");
                    sql.Append(" WHERE ");
                }


                if (emissao == true)
                    sql.Append(" ENTRADA.DT_EMISSAO >= {0} and ENTRADA.DT_EMISSAO <= {1} ");
                else
                    sql.Append(" ENTRADA.DT_ENTRADA >= {0} and ENTRADA.DT_ENTRADA <= {1} ");

                sql.Append(" and ENTRADA.DELETADO <> 'V' ");
                sql.Append(" and ENTRADA.NOTA_FECHADA = 'V' ");

                if (fornecedorId > 0)
                    sql.Append(" AND FORNECEDORES.CD_FORNECEDOR = " + fornecedorId);

                if (bonificacao == true)
                    sql.Append(" and (coalesce(ENTRADA.NF_BONIFICACAO, 'F') = 'V' or coalesce(ENTRADA.NF_BONIFICACAO, 'F') = 'F') ");
                else
                    sql.Append(" and coalesce(ENTRADA.NF_BONIFICACAO, 'F') <> 'V' ");

                if (modelo > 0)
                {
                    if (modelo == 55)
                    {
                        sql.Append(" and ENTRADA.MODELO = " + "'" + modelo + "'");
                    }
                    else
                    {
                        sql.Append(" and ENTRADA.MODELO = 01 ");
                    }
                }


                DateTime dataInicio = new DateTime(DateTime.Parse(inicio).Year, DateTime.Parse(inicio).Month, DateTime.Parse(inicio).Day, 0, 0, 1);
                DateTime dataFim = new DateTime(DateTime.Parse(fim).Year, DateTime.Parse(fim).Month, DateTime.Parse(fim).Day, 23, 59, 59);

                var retorno = _Context.RelatoriosE01.FromSqlRaw(@sql.ToString(), dataInicio, dataFim).ToList();
                return retorno;

            }
            catch (Exception)
            {
                return new List<RelatorioE01DTO>();
            }


        }


        [HttpGet("exportar")]
        public IActionResult ExportarRelatorio([FromQuery] List<int> lojaId, [FromQuery] bool? agrupado, [FromQuery] int? modelo, [FromQuery] bool? emissao, [FromQuery] bool? bonificacao, [FromQuery] int? fornecedorId, [FromQuery] string inicio, [FromQuery] string fim, [FromQuery] int ordenacao = 0, [FromQuery] string ascdesc = "ASC")
        {
            try
            {
                var resultado = ObterDados(lojaId, agrupado, modelo, emissao, bonificacao, fornecedorId, inicio, fim, ordenacao, ascdesc);


                StringBuilder relatorio = new StringBuilder();


                if (agrupado != true)
                {
                    relatorio.AppendLine("Data Emissão;Data Cadastro;Data Entrada;Cód Forn.;Fornecedor;Nº NF; Valor Produtos;Valor Nota;Nº da Parcela;Valor da Parcela;Vencimento");
                }
                else
                {
                    relatorio.AppendLine("Data Emissão;Data Cadastro;Data Entrada;Cód Forn.;Fornecedor;Nº NF;Valor;Valor Nota");
                }

                foreach (var item in resultado)
                {
                    relatorio.Append($"\"{item.Emissao}\";");
                    relatorio.Append($"\"{item.Cadastro}\";");
                    relatorio.Append($"\"{item.Entrada}\";");
                    relatorio.Append($"\"{item.FornecedorId}\";");
                    relatorio.Append($"\"{item.Fornecedor}\";");
                    relatorio.Append($"\"{item.NF}\";");
                    if (agrupado != true)
                    {
                        relatorio.Append($"\"{item.ValorProduto}\";");
                        relatorio.Append($"\"{item.ValorTotal}\";");
                        relatorio.Append($"\"{item.Parcela}\";");
                        relatorio.Append($"\"{item.ValorParcela}\";");
                        relatorio.Append($"\"{item.Vencimento}\";");

                    }
                    else
                    {
                        relatorio.Append($"\"{item.ValorProduto}\";");
                        relatorio.Append($"\"{item.ValorTotal}\";");
                    }
               
                    relatorio.AppendLine("");
                }



                string nomeArquivo = $"Relatorio_E01_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
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