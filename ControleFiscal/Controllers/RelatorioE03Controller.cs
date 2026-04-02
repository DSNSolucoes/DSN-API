using ControleFiscal.Domain.DTO.Relatorio;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Text; 

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatorioE03Controller : ControllerBase
    {

        private readonly ILogger<RelatorioE03Controller> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public RelatorioE03Controller(ILogger<RelatorioE03Controller> logger, ContextControleFiscalContext Context, ContextLocalContext ContextoLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextoLocal;
        }

        [HttpGet]
        public ActionResult<List<RelatorioE03DTO>> Obter([FromQuery] List<int> lojaId, int notaId)
        {
            try
            {
                var retorno = ObterDados(lojaId, notaId);
                return Ok(retorno);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }


        }



        private List<RelatorioE03DTO> ObterDados(List<int> lojaId, int notaId)
        {
            try
            {
                var nomeLoja = _Context.ConexaoCliente(lojaId.First(), _ContextLocal);

                StringBuilder sql = new StringBuilder();

                sql.Append("SELECT ");
                sql.Append("    ENTRADA_ITEM.sequencial AS Sequencial, ");
                sql.Append("    PRODUTOS.cod_barras AS CodigoBarras, ");
                sql.Append("    PRODUTOS.unidade AS Unidade, ");
                sql.Append("    PRODUTOS.nm_produto AS NomeProduto, ");
                sql.Append("    CAST(ENTRADA_ITEM.precocusto AS numeric(15,2)) AS PrecoCusto, ");
                sql.Append("    CAST(ENTRADA_ITEM.margemlucro AS numeric(15,2)) AS MargemLucro, ");
                sql.Append("    CAST(ENTRADA_ITEM.precovenda AS numeric(15,2)) AS PrecoVenda, ");
                sql.Append("    ENTRADA_ITEM.quantidade AS Quantidade, ");
                sql.Append("    CAST(ENTRADA_ITEM.undconversao AS numeric(15,2)) AS UnidadeConversao, ");
                sql.Append("    CAST(ENTRADA_ITEM.valortotal AS numeric(15,2)) AS ValorTotal, ");
                sql.Append("    ENTRADA.NUM_DOCUMENTO AS NumeroDocumento, ");
                sql.Append("    ENTRADA.CD_FORNECEDOR AS CodigoFornecedor, ");
                sql.Append("    ENTRADA.NM_FORNECEDOR AS NomeFornecedor, ");
                sql.Append("    ENTRADA.CNPJ AS Cnpj, ");
                sql.Append("    ENTRADA.CFOP AS Cfop, ");
                sql.Append("    CAST(ENTRADA.desconto_total AS numeric(15,2)) AS DescontoTotal, ");
                sql.Append("    ENTRADA.dt_cadastro AS DataCadastro, ");
                sql.Append("    ENTRADA.dt_emissao AS DataEmissao, ");
                sql.Append("    ENTRADA.dt_entrada AS DataEntrada, ");
                sql.Append("    ENTRADA.ultimaalteracao AS UltimaAlteracao, ");
                sql.Append("    CAST(ENTRADA.icms_base AS numeric(15,2)) AS IcmsBase, ");
                sql.Append("    CAST(ENTRADA.icms_valor AS numeric(15,2)) AS IcmsValor, ");
                sql.Append("    CAST(ENTRADA.icms_basesubst AS numeric(15,2)) AS IcmsBaseSubstituicao, ");
                sql.Append("    CAST(ENTRADA.icms_valorsubst AS numeric(15,2)) AS IcmsValorSubstituicao, ");
                sql.Append("    CAST(ENTRADA.vr_total_prod AS numeric(15,2)) AS ValorTotalProdutos, ");
                sql.Append("    CAST(ENTRADA.valor_frete AS numeric(15,2)) AS ValorFrete, ");
                sql.Append("    CAST(ENTRADA.valor_seguro AS numeric(15,2)) AS ValorSeguro, ");
                sql.Append("    CAST(ENTRADA.outras_despesas AS numeric(15,2)) AS OutrasDespesas, ");
                sql.Append("    CAST(ENTRADA.valor_ipi AS numeric(15,2)) AS ValorIpi, ");
                sql.Append("    CAST(ENTRADA.valor_total_nota AS numeric(15,2)) AS ValorTotalNota, ");
                sql.Append("    PRODUTOS.classfiscal AS ClassificacaoFiscal, ");
                sql.Append("    PRODUTOS.cst AS Cst ");
                sql.Append("FROM ");
                sql.Append("    ENTRADA_ITEM ");
                sql.Append("LEFT OUTER JOIN ENTRADA ON ENTRADA_ITEM.CD_NOTA = ENTRADA.CD_NOTA ");
                sql.Append("LEFT OUTER JOIN PRODUTOS ON PRODUTOS.CD_PRODUTO = ENTRADA_ITEM.CD_PRODUTO ");
                sql.Append("WHERE ");
                sql.Append("    ENTRADA.cd_nota = {0}");
                sql.Append("    AND ENTRADA.DELETADO <> 'V' ");
                sql.Append("    AND ENTRADA_ITEM.DELETADO <> 'V';");
                 

                var retorno = _Context.RelatoriosE03.FromSqlRaw(@sql.ToString(),notaId).ToList();
                return retorno;

            }
            catch (Exception)
            {
                return new List<RelatorioE03DTO>();
            }


        }


        [HttpGet("exportar")]
        public IActionResult ExportarRelatorio([FromQuery] List<int> lojaId,  int notaId)
        {
            try
            {
                var resultado = ObterDados(lojaId, notaId);


                StringBuilder relatorio = new StringBuilder();


                relatorio.AppendLine("Seq.;Cód Barras;Und.;Descrição;Vlr. Custo;Margem %;Vlr Venda;Qtde.;U. Conv.;Vlr Total");

                foreach (var item in resultado)
                {
                    relatorio.Append($"\"{item.Sequencial}\";");
                    relatorio.Append($"\"{item.CodigoBarras}\";");
                    relatorio.Append($"\"{item.Unidade}\";"); 
                    relatorio.Append($"\"{item.NomeProduto}\";"); 
                    relatorio.Append($"\"{item.PrecoCusto}\";"); 
                    relatorio.Append($"\"{item.MargemLucro}\";"); 
                    relatorio.Append($"\"{item.PrecoVenda}\";"); 
                    relatorio.Append($"\"{item.Quantidade}\";"); 
                    //relatorio.Append($"\"{item.UndConversao}\";"); 
                    relatorio.Append($"\"{item.ValorTotal}\";"); 
                    relatorio.AppendLine("");
                }



                string nomeArquivo = $"Relatorio_E03_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
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