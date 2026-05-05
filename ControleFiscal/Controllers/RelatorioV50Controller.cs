using ControleFiscal.Domain.DTO.Relatorio;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatorioV50Controller : ControllerBase
    {

        private readonly ILogger<RelatorioV50Controller> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public RelatorioV50Controller(ILogger<RelatorioV50Controller> logger, ContextControleFiscalContext Context, ContextLocalContext ContextoLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextoLocal;
        }


        private StringBuilder SQLV50(int lojaId,
                                          int? grupoId,
                                          int? produtoId,
                                          int? fornecedorId,
                                          string? inicio,
                                          string? fim,
                                          int? ordenacao,
                                          string? ascdesc = "ASC")
        {

            var nomeLoja = _Context.ConexaoCliente(lojaId, _ContextLocal);

            StringBuilder sql = new StringBuilder();

            sql.Append("WITH ITEMS AS ( ");
            sql.Append("  SELECT DISTINCT I.CD_PRODUTO ");
            sql.Append("  FROM PEDIDO_ITEM I ");
            sql.Append("  JOIN PEDIDO P ON I.NUM_DOCUMENTO = P.NUM_DOCUMENTO ");
            sql.Append("  WHERE P.dt_fechamento BETWEEN {0} AND {1} ");
            sql.Append("  AND P.TIPO_DOCUMENTO = 'V' ");
            sql.Append("  AND COALESCE(P.CANCELADO, 'F') <> 'V' ");
            sql.Append("  AND COALESCE(I.CANCELADO, 'F') <> 'V' ");
            sql.Append("), ");
            sql.Append("TOT AS ( ");
            sql.Append("  SELECT I.cd_produto, ");
            sql.Append("    SUM(I.SUBTOTAL) AS VALORTOTAL, ");
            sql.Append("    SUM(I.QUANTIDADE) AS QUANTIDADETOTAL, ");
            sql.Append("    avg(I.PRECO) AS PRECOMEDIO, ");
            sql.Append("   avg(((I.PRECO-coalesce(I.PRECOCUSTO,1))/COALESCE(iif(I.PRECOCUSTO = 0,1,I.PRECOCUSTO ),1))*100) AS MARKUP  ");
            sql.Append("  FROM PEDIDO_ITEM I ");
            sql.Append("  JOIN PEDIDO P ON P.NUM_DOCUMENTO = I.NUM_DOCUMENTO ");
            sql.Append("  WHERE P.dt_fechamento BETWEEN {0} AND {1} and P.TIPO_DOCUMENTO = 'V' ");
            sql.Append("    AND COALESCE(P.CANCELADO, 'F') <> 'V' ");
            sql.Append("    AND COALESCE(I.CANCELADO, 'F') <> 'V' ");
            sql.Append("    AND COALESCE(I.PRECO, 0) <> 0 ");
            sql.Append("  GROUP BY I.cd_produto ");
            sql.Append("), ");
            sql.Append("ENTRADATOTAL AS ( ");
            sql.Append("SELECT EI.CD_PRODUTO, SUM(EI.QUANTIDADE * coalesce(EI.UNDCONVERSAO,0)) as QUANTIDADECOMPRADA  FROM ENTRADA E ");
            sql.Append("LEFT JOIN ENTRADA_ITEM EI ON E.CD_NOTA = EI.CD_NOTA ");
            sql.Append("WHERE E.DT_ENTRADA BETWEEN {0} and {1} AND ");
            sql.Append("COALESCE(EI.DELETADO, 'F') <> 'V' AND ");
            sql.Append("COALESCE(E.DELETADO, 'F') <> 'V'  ");

            if (fornecedorId > 0)
            {
                sql.Append("and E.CNPJ = {3} ");
            }

            sql.Append("group by 1 ");
            sql.Append(") ");
            sql.Append("SELECT DISTINCT ");
            sql.Append("  PR.COD_BARRAS as codBarras , ");
            sql.Append("  PF.COD_PROD_FORNEC as codProdFornec , ");
            sql.Append("  PR.NM_PRODUTO as nomeProduto, ");
            sql.Append("  ENT.O_VALOR_UNIT as valorUnitario, ");
            sql.Append("  ENT.O_IPI as ipi, ");
            sql.Append("  ENT.O_ST as st, ");
            sql.Append("  ENT.O_PRECO_CUSTO as precoCusto , ");
            sql.Append("  ENT.O_DATAULTIMACOMPRA as dataUltimaCompra , ");
            sql.Append("  ENT.O_QTDULTIMACOMPRA  as qtdUltimaCompra, ");
            sql.Append("  TOT.QUANTIDADETOTAL  as quantidadeTotal, ");
            sql.Append("  E.ESTOQUEATUAL as estoqueAtual, ");
            sql.Append("  TOT.PRECOMEDIO as precoMedio , ");
            sql.Append("  TOT.VALORTOTAL as valorTotal, ");
            sql.Append("  REF.MEDIAVENDA as mediavenda, ");
            sql.Append("  TOT.MARKUP as markup, ");
            sql.Append("  COALESCE(PR.CX_UNDCONVERSAO,0) as UNDCONVERSAO, ");
            sql.Append("  '" + nomeLoja + "' as loja, ");
            sql.Append("  COALESCE(ENTRADATOTAL.QUANTIDADECOMPRADA,0) as QUANTIDADECOMPRADA, ");
            sql.Append("  ((COALESCE(TOT.QUANTIDADETOTAL,1) / COALESCE(ENTRADATOTAL.QUANTIDADECOMPRADA,1)) * 100) as PERCENTUALVENDIDO ");
            sql.Append("FROM ITEMS ");
            sql.Append("JOIN PRODUTOS PR ON PR.CD_PRODUTO = ITEMS.CD_PRODUTO ");
            sql.Append("JOIN PRODUTOS_REF REF ON REF.CD_PRODUTO = ITEMS.CD_PRODUTO ");
            sql.Append("JOIN TOT ON TOT.CD_PRODUTO = ITEMS.CD_PRODUTO ");
            sql.Append("LEFT JOIN ENTRADATOTAL ON  ENTRADATOTAL.CD_PRODUTO = ITEMS.CD_PRODUTO ");
            sql.Append("JOIN SP_DADOS_ULTIMA_ENTRADA(ITEMS.CD_PRODUTO) ENT ON 1=1 ");
            sql.Append("JOIN SP_PEGAESTOQUE(ITEMS.CD_PRODUTO, 0, 0) E ON 1=1 ");
            sql.Append("LEFT JOIN PRODFORNEC PF ON PF.CD_PRODUTO = ITEMS.CD_PRODUTO AND PF.CD_FORNECEDOR = ENT.O_ULTIMOFORNECEDOR ");


            if (grupoId > 0)
                sql.Append("left join SP_PERTENCE_AO_GRUPO(PR.CD_GRUPO, " + grupoId + ") G on (1=1)");

            sql.Append("WHERE (1=1) ");

            if (produtoId > 0)
                sql.Append("AND PR.cd_produto = " + produtoId);

            if (fornecedorId > 0)
                sql.Append(" AND ENT.O_ULTIMOFORNECEDOR = {2}");

            if (grupoId > 0)
            {
                sql.Append(" AND ((SELECT PERTENCE FROM SP_PERTENCE_AO_GRUPO(PR.CD_GRUPO, " + grupoId + ")) = 'V')                 ");
                sql.Append(" AND G.Pertence = 'V'                ");
            }
             
            sql.Append(" ORDER BY " + ordenacao.GetValueOrDefault(1) + ascdesc);

            return sql;
        }



        [HttpGet]
        public ActionResult<List<RelatorioV50DTO>> Obter([FromQuery] List<int> lojaId,
                                                         [FromQuery] int? grupoId,
                                                         [FromQuery] int? produtoId,
                                                         [FromQuery] int? fornecedorId,
                                                         [FromQuery] string inicio,
                                                         [FromQuery] string fim,
                                                         [FromQuery] int ordenacao = 0,
                                                         [FromQuery] string ascdesc = "ASC")
        {
            try
            {
                _Context.ConexaoCliente(lojaId.First(), _ContextLocal);

                var fornecedorCNPJ = _Context.Fornecedores.FirstOrDefault(x => x.CdFornecedor == fornecedorId)?.CpfCnpj;

                var sql = SQLV50(lojaId.First(), grupoId, produtoId, fornecedorId, inicio, fim, ordenacao, ascdesc);


                DateTime dataInicio = new DateTime(DateTime.Parse(inicio).Year, DateTime.Parse(inicio).Month, DateTime.Parse(inicio).Day, 0, 0, 0);
                DateTime dataFim = new DateTime(DateTime.Parse(fim).Year, DateTime.Parse(fim).Month, DateTime.Parse(fim).Day, 23, 59, 59);

                if (fornecedorId > 0)
                {
                    var retorno = _Context.RelatoriosV50.FromSqlRaw(@sql.ToString(), dataInicio, dataFim, fornecedorId, fornecedorCNPJ).ToList();
                    return Ok(retorno);
                }
                else
                {
                    var retorno = _Context.RelatoriosV50.FromSqlRaw(@sql.ToString(), dataInicio, dataFim).ToList();
                    return Ok(retorno);
                }

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }


        }

        [HttpGet("ObterDoGrupo")]
        public IActionResult ObterTodasLojas([FromQuery] List<int> lojaId,
                                             [FromQuery] int? grupoId,
                                             [FromQuery] int? produtoId,
                                             [FromQuery] int? fornecedorId,
                                             [FromQuery] string inicio,
                                             [FromQuery] string fim,
                                             [FromQuery] int empresaFornecedor,
                                             [FromQuery] int ordenacao = 0,
                                             [FromQuery] string ascdesc = "ASC")
        {

            var resultado = ObterV50Agrupado(lojaId, grupoId, produtoId, fornecedorId, inicio, fim, empresaFornecedor,ordenacao, ascdesc);

            if (!resultado.Item1)
            {
                return Ok(resultado.Item3);
            }
            else
            {
                return BadRequest(resultado.Item2);
            }
        }

        private Tuple<bool, string, List<RelatorioV50DTO>?> ObterV50Agrupado(List<int> lojaId,
                                                                               int? grupoId,
                                                                               int? produtoId,
                                                                               int? fornecedorId,
                                                                               string inicio,
                                                                               string fim,
                                                                               int empresaFornecedor,
                                                                               int ordenacao = 0,
                                                                               string ascdesc = "ASC")
        {
            try
            {
                _Context.ConexaoCliente(empresaFornecedor, _ContextLocal);

                string fornecedorCNPJ = "";

                if (fornecedorId is not null && fornecedorId > 0)
                {
                    fornecedorCNPJ = _Context.Fornecedores.First(x => x.CdFornecedor == fornecedorId).CpfCnpj;
                    fornecedorCNPJ = Regex.Replace(fornecedorCNPJ, @"[^a-zA-Z0-9]", "");
                }

                var lojas = _ContextLocal.Empresas.Where(x => lojaId.Contains(x.Id));//Para nao pegar nada do Entrada
                List<RelatorioV50DTO> retorno = new();

                foreach (var item in lojas)
                {
                    try
                    {
                        ContextControleFiscalContext contextLoja = new ContextControleFiscalContext();
                        contextLoja.ConexaoCliente(_ContextLocal, item.Id);

                        int? fornecedorIdLoja = 0;

                        if (fornecedorId is not null && fornecedorId > 0)
                        {
                            fornecedorIdLoja = contextLoja.Fornecedores?.FirstOrDefault(x => x.CpfCnpj == fornecedorCNPJ)?.CdFornecedor;
                        }


                        var sql = SQLV50(item.Id, grupoId, produtoId, fornecedorIdLoja.GetValueOrDefault(0), inicio, fim, ordenacao, ascdesc);

                        DateTime dataInicio = new DateTime(DateTime.Parse(inicio).Year, DateTime.Parse(inicio).Month, DateTime.Parse(inicio).Day, 0, 0, 0);
                        DateTime dataFim = new DateTime(DateTime.Parse(fim).Year, DateTime.Parse(fim).Month, DateTime.Parse(fim).Day, 23, 59, 59);


                        if (fornecedorId is not null && fornecedorId > 0)
                        {
                            if (fornecedorIdLoja.GetValueOrDefault(0) > 0)
                            {
                                var lista = contextLoja.RelatoriosV50.FromSqlRaw(@sql.ToString(), dataInicio, dataFim, fornecedorIdLoja, fornecedorCNPJ).ToList().OrderBy(r => r.codBarras);
                                retorno.AddRange(lista);
                            }

                        }
                        else
                        {
                            var lista = contextLoja.RelatoriosV50.FromSqlRaw(@sql.ToString(), dataInicio, dataFim).ToList().OrderBy(r => r.codBarras);
                            retorno.AddRange(lista);
                        }
                       
                            #region SQL
                            //StringBuilder sql = new StringBuilder();

                            //sql.Append("WITH ITEMS AS ( ");
                            //sql.Append("  SELECT DISTINCT I.CD_PRODUTO ");
                            //sql.Append("  FROM PEDIDO_ITEM I ");
                            //sql.Append("  JOIN PEDIDO P ON I.NUM_DOCUMENTO = P.NUM_DOCUMENTO ");
                            //sql.Append("  WHERE P.dt_fechamento BETWEEN {0} AND {1} ");
                            //sql.Append("  AND P.TIPO_DOCUMENTO = 'V' ");
                            //sql.Append("  AND COALESCE(P.CANCELADO, 'F') <> 'V' ");
                            //sql.Append("  AND COALESCE(I.CANCELADO, 'F') <> 'V' ");
                            //sql.Append("), ");
                            //sql.Append("TOT AS ( ");
                            //sql.Append("  SELECT I.cd_produto, ");
                            //sql.Append("    SUM(I.SUBTOTAL) AS VALORTOTAL, ");
                            //sql.Append("    SUM(I.QUANTIDADE) AS QUANTIDADETOTAL, ");
                            //sql.Append("    avg(I.PRECO) AS PRECOMEDIO, ");
                            //sql.Append("   avg(((I.PRECO-coalesce(I.PRECOCUSTO,1))/COALESCE(iif(I.PRECOCUSTO = 0,1,I.PRECOCUSTO ),1))*100) AS MARKUP  ");
                            //sql.Append("  FROM PEDIDO_ITEM I ");
                            //sql.Append("  JOIN PEDIDO P ON P.NUM_DOCUMENTO = I.NUM_DOCUMENTO ");
                            //sql.Append("  WHERE P.dt_fechamento BETWEEN {0} AND {1} and P.TIPO_DOCUMENTO = 'V' ");
                            //sql.Append("    AND COALESCE(P.CANCELADO, 'F') <> 'V' ");
                            //sql.Append("    AND COALESCE(I.CANCELADO, 'F') <> 'V' ");
                            //sql.Append("    AND COALESCE(I.PRECO, 0) <> 0 ");
                            //sql.Append("  GROUP BY I.cd_produto ");
                            //sql.Append("), ");
                            //sql.Append("ENTRADATOTAL AS ( ");
                            //sql.Append("SELECT EI.CD_PRODUTO, SUM(EI.QUANTIDADE * coalesce(EI.UNDCONVERSAO,0)) as QUANTIDADECOMPRADA FROM ENTRADA E ");
                            //sql.Append("LEFT JOIN ENTRADA_ITEM EI ON E.CD_NOTA = EI.CD_NOTA ");
                            //sql.Append("WHERE E.DT_ENTRADA BETWEEN {0} and {1} AND ");
                            //sql.Append("COALESCE(EI.DELETADO, 'F') <> 'V' AND ");
                            //sql.Append("COALESCE(E.DELETADO, 'F') <> 'V'  ");

                            //if (fornecedorId > 0)
                            //{
                            //    sql.Append("and e.cd_fornecedor = {2} ");
                            //}

                            //sql.Append("group by 1 ");
                            //sql.Append(") ");
                            //sql.Append("SELECT DISTINCT ");
                            //sql.Append("  PR.COD_BARRAS as codBarras , ");
                            //sql.Append("  PF.COD_PROD_FORNEC as codProdFornec , ");
                            //sql.Append("  PR.NM_PRODUTO as nomeProduto, ");
                            //sql.Append("  ENT.O_VALOR_UNIT as valorUnitario, ");
                            //sql.Append("  ENT.O_IPI as ipi, ");
                            //sql.Append("  ENT.O_ST as st, ");
                            //sql.Append("  ENT.O_PRECO_CUSTO as precoCusto , ");
                            //sql.Append("  ENT.O_DATAULTIMACOMPRA as dataUltimaCompra , ");
                            //sql.Append("  ENT.O_QTDULTIMACOMPRA  as qtdUltimaCompra, ");
                            //sql.Append("  TOT.QUANTIDADETOTAL  as quantidadeTotal, ");
                            //sql.Append("  E.ESTOQUEATUAL as estoqueAtual, ");
                            //sql.Append("  TOT.PRECOMEDIO as precoMedio , ");
                            //sql.Append("  TOT.VALORTOTAL as valorTotal, ");
                            //sql.Append("  TOT.MARKUP as markup, ");
                            //sql.Append("  COALESCE(PR.CX_UNDCONVERSAO,0) as UNDCONVERSAO, ");
                            //sql.Append("  '" + item.nome + "' as loja, ");
                            //sql.Append("  ENTRADATOTAL.QUANTIDADECOMPRADA as QUANTIDADECOMPRADA, ");
                            //sql.Append("  ((COALESCE(TOT.QUANTIDADETOTAL,1) / COALESCE(ENTRADATOTAL.QUANTIDADECOMPRADA,1)) * 100) as PERCENTUALVENDIDO ");
                            //sql.Append("FROM ITEMS ");
                            //sql.Append("JOIN PRODUTOS PR ON PR.CD_PRODUTO = ITEMS.CD_PRODUTO ");
                            //sql.Append("JOIN TOT ON TOT.CD_PRODUTO = ITEMS.CD_PRODUTO ");
                            //sql.Append("JOIN ENTRADATOTAL ON  ENTRADATOTAL.CD_PRODUTO = ITEMS.CD_PRODUTO ");
                            //sql.Append("JOIN SP_DADOS_ULTIMA_ENTRADA(ITEMS.CD_PRODUTO) ENT ON 1=1 ");
                            //sql.Append("JOIN SP_PEGAESTOQUE(ITEMS.CD_PRODUTO, 0, 0) E ON 1=1 ");
                            //sql.Append("LEFT JOIN PRODFORNEC PF ON PF.CD_PRODUTO = ITEMS.CD_PRODUTO AND PF.CD_FORNECEDOR = ENT.O_ULTIMOFORNECEDOR ");


                            //if (grupoId > 0)
                            //    sql.Append("left join SP_PERTENCE_AO_GRUPO(PR.CD_GRUPO, " + grupoId + ") G on (1=1)");
                            //sql.Append("WHERE (1=1) ");

                            //if (produtoId > 0)
                            //    sql.Append("AND PR.cd_produto = " + produtoId);

                            //if (fornecedorId > 0)
                            //    sql.Append("AND ENT.O_ULTIMOFORNECEDOR = {2}");

                            //if (grupoId > 0)
                            //{
                            //    sql.Append("AND ((SELECT PERTENCE FROM SP_PERTENCE_AO_GRUPO(PR.CD_GRUPO, " + grupoId + ")) = 'V')                 ");
                            //    sql.Append("AND G.Pertence = 'V'                ");
                            //}

                            //sql.Append(" ORDER BY " + ordenacao + ascdesc);

                            #endregion
                         
                          
                
                    }
                    catch (Exception)
                    {
                    }
                }
                retorno = retorno.OrderBy(x => x.codBarras).ToList();
                return new Tuple<bool, string, List<RelatorioV50DTO>?>(false, string.Empty, retorno);
            }
            catch (Exception e)
            {
                var msgErro = e.Message + e.StackTrace;
                return new Tuple<bool, string, List<RelatorioV50DTO>?>(true, msgErro, null);
            }

        }


        [HttpGet("exportarAgrupado")]
        public IActionResult ExportarRelatorioAgrupado([FromQuery] List<int> lojaId,
                                                       [FromQuery] int? grupoId,
                                                       [FromQuery] int? produtoId,
                                                       [FromQuery] int? fornecedorId,
                                                       [FromQuery] string inicio,
                                                       [FromQuery] string fim,
                                                       [FromQuery] int empresaFornecedor,
                                                       [FromQuery] int ordenacao = 0,
                                                       [FromQuery] string ascdesc = "ASC")
        {
            try
            {
                var resultado = ObterV50Agrupado(lojaId, grupoId, produtoId, fornecedorId, inicio, fim, empresaFornecedor, ordenacao, ascdesc);

                if (resultado.Item1 || resultado.Item3 == null)
                {
                    return BadRequest(resultado.Item2);
                }

                StringBuilder relatorio = new StringBuilder();

                relatorio.AppendLine("Nome Produto;Cód de Barras;Cód do Fornecedor;Valor Unitário;IPI;ST;Preço de Custo;Caixa Master;Data da Última Compra;Quantidade da Última Compra;Quantidade Venda;Estoque Atual;Preço Médio;SubTotal Vendas;Markup;Quantidade Total Comprada;% Venda Total;Loja; Média Venda");

                foreach (var item in resultado.Item3)
                {
                    relatorio.Append($"\"{item.nomeProduto}\";");
                    relatorio.Append($"\"{item.codBarras}\";");
                    relatorio.Append($"\"{item.codProdFornec}\";");
                    relatorio.Append($"\"{item.valorUnitario}\";");
                    relatorio.Append($"\"{item.ipi}\";");
                    relatorio.Append($"\"{item.st}\";");
                    relatorio.Append($"\"{item.precoCusto}\";");
                    relatorio.Append($"\"{item.undConversao}\";");
                    relatorio.Append($"\"{item.dataUltimaCompra}\";");
                    relatorio.Append($"\"{item.qtdUltimaCompra}\";");
                    relatorio.Append($"\"{item.quantidadeTotal}\";");
                    relatorio.Append($"\"{item.estoqueAtual}\";");
                    relatorio.Append($"\"{item.precoMedio}\";");
                    relatorio.Append($"\"{item.valorTotal}\";");
                    relatorio.Append($"\"{item.markup}\";");
                    relatorio.Append($"\"{item.quantidadecomprada}\";");
                    relatorio.Append($"\"{item.percentualvendido}\";");
                    relatorio.Append($"\"{item.Loja}\";");
                    relatorio.Append($"\"{item.mediaVenda}\";");
                    relatorio.AppendLine("");
                }

                string nomeArquivo = $"Relatorio_V50_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(relatorio.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpGet("exportar")]
        public IActionResult ExportarRelatorio([FromQuery] int lojaId, [FromQuery] int? grupoId, [FromQuery] int? produtoId, [FromQuery] int? fornecedorId, [FromQuery] string inicio, [FromQuery] string fim, [FromQuery] int ordenacao = 0, [FromQuery] string ascdesc = "ASC")
        {
            try
            {
                _Context.ConexaoCliente(lojaId, _ContextLocal);
                var fornecedorCNPJ = _Context.Fornecedores.First(x => x.CdFornecedor == fornecedorId).CpfCnpj;
                var sql = SQLV50(lojaId, grupoId, produtoId, fornecedorId, inicio, fim, ordenacao, ascdesc);

                DateTime dataInicio = new DateTime(DateTime.Parse(inicio).Year, DateTime.Parse(inicio).Month, DateTime.Parse(inicio).Day, 0, 0, 0);
                DateTime dataFim = new DateTime(DateTime.Parse(fim).Year, DateTime.Parse(fim).Month, DateTime.Parse(fim).Day, 23, 59, 59);

                List<RelatorioV50DTO> resultado;

                if (fornecedorId > 0)
                {
                    resultado = _Context.RelatoriosV50.FromSqlRaw(@sql.ToString(), dataInicio, dataFim, fornecedorId, fornecedorCNPJ).ToList();
                }
                else
                {
                    resultado = _Context.RelatoriosV50.FromSqlRaw(@sql.ToString(), dataInicio, dataFim).ToList();
                }


                StringBuilder relatorio = new StringBuilder();

                relatorio.AppendLine("Nome Produto;Cód de Barras;Cód do Fornecedor;Valor Unitário;IPI;ST;Preço de Custo;Data da Última Compra;Quantidade da Última Compra;Quantidade Venda;Estoque Atual;Preço Médio;SubTotal Vendas;Markup;Quantidade Total Comprada;% Venda Total; Média Venda");

                foreach (var item in resultado)
                {
                    relatorio.Append($"\"{item.nomeProduto}\";");
                    relatorio.Append($"\"{item.codBarras}\";");
                    relatorio.Append($"\"{item.codProdFornec}\";");
                    relatorio.Append($"\"{item.valorUnitario}\";");
                    relatorio.Append($"\"{item.ipi}\";");
                    relatorio.Append($"\"{item.st}\";");
                    relatorio.Append($"\"{item.precoCusto}\";");
                    relatorio.Append($"\"{item.dataUltimaCompra}\";");
                    relatorio.Append($"\"{item.qtdUltimaCompra}\";");
                    relatorio.Append($"\"{item.quantidadeTotal}\";");
                    relatorio.Append($"\"{item.estoqueAtual}\";");
                    relatorio.Append($"\"{item.precoMedio}\";");
                    relatorio.Append($"\"{item.valorTotal}\";");
                    relatorio.Append($"\"{item.markup}\";");
                    relatorio.Append($"\"{item.quantidadecomprada}\";");
                    relatorio.Append($"\"{item.percentualvendido}\";");
                    relatorio.Append($"\"{item.mediaVenda}\";");
                    relatorio.AppendLine("");
                }



                string nomeArquivo = $"Relatorio_V50_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
                byte[] bytes = Encoding.UTF8.GetBytes(relatorio.ToString());
                bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
                return File(bytes, "text/csv", nomeArquivo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpGet("RemoverVinculoLojasGrupo")]
        public IActionResult RemoverVinculoLojas()
        {
            return RemoverVinculo();
        }



        private IActionResult RemoverVinculo()
        {
            var sql = " select ent.cd_fornecedor as fornecedorId , item.cd_produto as produtoId " +
                      " from entrada ent " +
                      " left join entrada_item item on item.cd_nota = ent.cd_nota " +
                      " where " +
                      " coalesce(item.deletado, 'F') <> 'V' and " +
                      " coalesce(ent.deletado, 'F') <> 'V' and " +
                      " ent.cnpj not in ('28831999000130', '59841838000150', '59841838000150') and " +
                      " item.cd_produto in (" +
                      " select ei.cd_produto from entrada_item ei where ei.cd_nota in (" +
                      "select e.cd_nota from entrada e where e.cnpj in  ('28831999000130', '59841838000150', '59841838000150') ) )";



            var lojas = _ContextLocal.Empresas.Where(x => x.Id < 999);

            foreach (var item in lojas)
            {
                try
                {
                    using var contextLoja = new ContextControleFiscalContext();

                    contextLoja.ConexaoCliente(_ContextLocal, item.Id);

                    try
                    {
                        var resultado = contextLoja.AtualizarVinculo
                       .FromSqlRaw(sql)
                       .ToList();

                        if (!resultado.Any())
                            return NoContent();

                        var fornecedorPorProduto = resultado
                            .GroupBy(x => x.ProdutoId)
                            .ToDictionary(
                                g => g.Key,
                                g => g.First().FornecedorId
                            );

                        var produtoIds = fornecedorPorProduto.Keys.ToList();

                        var produtosAtualizar = contextLoja.ProdutosRef
                            .Where(x => produtoIds.Contains(x.CdProduto))
                            .ToList();

                        foreach (var produto in produtosAtualizar)
                        {
                            if (fornecedorPorProduto.TryGetValue(produto.CdProduto, out var fornecedorId))
                            {
                                produto.Ultimofornecedor = fornecedorId;
                            }
                        }

                        contextLoja.ProdutosRef.UpdateRange(produtosAtualizar);
                        contextLoja.SaveChanges();
                    }
                    catch (Exception)
                    {

                    }

                }
                catch (Exception ex)
                {
                    BadRequest(ex);
                }
            }

            return Ok();

        }



        [HttpGet("RemoverMascaraCNPJ")]
        public IActionResult RemoverMascaraCNPJ()
        {
            var lojas = _ContextLocal.Empresas.Where(x => x.Id < 999);
            foreach (var item in lojas)
            {
                try
                {
                    using var contextLoja = new ContextControleFiscalContext();
                    contextLoja.ConexaoCliente(_ContextLocal, item.Id);
                    var fornecedores = contextLoja.Fornecedores.ToList();
                    foreach (var fornecedor in fornecedores)
                    {
                        if (!string.IsNullOrEmpty(fornecedor.CpfCnpj))
                        {
                            fornecedor.CpfCnpj = Regex.Replace(fornecedor.CpfCnpj, @"[^a-zA-Z0-9]", "");
                        }
                    }
                    contextLoja.Fornecedores.UpdateRange(fornecedores);
                    contextLoja.SaveChanges();
                }
                catch (Exception ex)
                {
                    BadRequest(ex);
                }
            }
            return Ok();
        }


      


        [HttpPost("AtualizarTriggerEntrada")] 
        public IActionResult RemoverMascaraCNPJEntradaPost()
        {
            var script = @"CREATE OR ALTER TRIGGER ENTRADA_BIU FOR ENTRADA
                                ACTIVE BEFORE INSERT OR UPDATE POSITION 0
                                as
                                begin
                                  if (GEN_ID(trigger_ativado, 0) > 0) then
                                  begin
                                    if ((new.CD_NOTA is null) or (new.CD_NOTA <> 0)) then  -- Se não é Modo Configuração Padrão
                                    begin
                                      /* Pega o código sequencial */
                                      new.CD_NOTA = coalesce(new.CD_NOTA, 0);
                                      while (new.CD_NOTA <= 0) do new.CD_NOTA = GEN_ID(GEN_ENTRADA, 1);
                                    end

                                --------------------------------------------------------------------------------
                                    if (coalesce (new.SINCRONIZANDO, 'F')= 'F') then
                                    begin
                                      new.ULTIMAALTERACAO = current_timestamp;
                                      if (new.DT_CADASTRO is null) then
                                      begin
                                        if (inserting) then new.DT_CADASTRO = current_timestamp;
                                        if (updating)  then new.DT_CADASTRO = old.DT_CADASTRO;
                                      end

                                      if ((new.DELETADO = 'V') and (coalesce(old.DELETADO, 'F') = 'F')) then
                                        new.DELETADO_DATAHORA = current_timestamp;
                                    end
                                    new.SINCRONIZANDO = 'F';

                                    if (new.CNPJ is not null) then
                                      new.CNPJ = replace(replace(replace(new.CNPJ, '.', ''), '/', ''), '-', '');
                                --------------------------------------------------------------------------------
                                  end
                                end";

           

            return Ok(ExecutarScript(script));

        }


        [HttpPost("UpdateEntrada")]
        public IActionResult UpdateEntrada()
        { 
            var script = "update entrada f set f.ultimaalteracao = current_timestamp"; 

            return Ok(ExecutarScript(script));

        }

        [HttpGet("AtualizarDadosUltimaEntrada")]
        public IActionResult AtualizarProcedureUltimaEntrada()
        {

            var script = @"
            CREATE OR ALTER PROCEDURE SP_DADOS_ULTIMA_ENTRADA (
                ICD_PRODUTO INTEGER)
            RETURNS (
                O_VALOR_UNIT NUMERIC(18,3),
                O_IPI NUMERIC(18,2),
                O_ST NUMERIC(18,2),
                O_PRECO_CUSTO NUMERIC(18,3),
                O_QTDULTIMACOMPRA NUMERIC(18,3),
                O_DATAULTIMACOMPRA TIMESTAMP,
                O_ULTIMOFORNECEDOR INTEGER)
            AS
            DECLARE VARIABLE ICD_NOTA INTEGER;
            BEGIN
                SELECT FIRST 1
                    E.CD_NOTA,
                    E.DT_FECHAMENTO
                FROM ENTRADA E
                JOIN ENTRADA_ITEM I ON I.CD_NOTA = E.CD_NOTA
                WHERE
                    I.CD_PRODUTO = :ICD_PRODUTO
                    AND COALESCE(I.DELETADO, 'F') <> 'V'
                    AND COALESCE(E.DELETADO, 'F') <> 'V'
                ORDER BY E.DT_FECHAMENTO DESC
                INTO :ICD_NOTA, :O_DATAULTIMACOMPRA;

                SELECT FIRST 1
                    E.DT_FECHAMENTO,
                    PR.ULTIMOFORNECEDOR,
                    EI.VALORUNITARIO,
                    EI.IPI_VALUNID,
                    COALESCE(EI.QUANTIDADE, 1) * COALESCE(EI.UNDCONVERSAO, 1),
                    IIF((COALESCE(EI.ICMSST_VAL, 1) <= 0), 1, COALESCE(EI.ICMSST_VAL, 1)) /
                        COALESCE(
                            IIF((COALESCE(EI.QUANTIDADE, 1) <= 0), 1, COALESCE(EI.QUANTIDADE, 1)) *
                            IIF((COALESCE(EI.UNDCONVERSAO, 1) <= 0), 1, COALESCE(EI.UNDCONVERSAO, 1)),
                        1),
                    COALESCE(EI.VALORUNITARIO, 0) +
                    COALESCE(EI.IPI_VALUNID, 0) +
                    (
                        IIF((COALESCE(EI.ICMSST_VAL, 1) <= 0), 1, COALESCE(EI.ICMSST_VAL, 1)) /
                        COALESCE(
                            IIF((COALESCE(EI.QUANTIDADE, 1) <= 0), 1, COALESCE(EI.QUANTIDADE, 1)) *
                            IIF((COALESCE(EI.UNDCONVERSAO, 1) <= 0), 1, COALESCE(EI.UNDCONVERSAO, 1)),
                        1)
                    )
                FROM ENTRADA E
                JOIN ENTRADA_ITEM EI ON E.CD_NOTA = EI.CD_NOTA
                JOIN PRODUTOS_REF PR ON PR.CD_PRODUTO = EI.CD_PRODUTO
                WHERE
                    E.CD_NOTA = :ICD_NOTA
                    AND EI.CD_PRODUTO = :ICD_PRODUTO
                    AND COALESCE(EI.DELETADO, 'F') <> 'V'
                    AND COALESCE(E.DELETADO, 'F') <> 'V'
                INTO
                    :O_DATAULTIMACOMPRA,
                    :O_ULTIMOFORNECEDOR,
                    :O_VALOR_UNIT,
                    :O_IPI,
                    :O_QTDULTIMACOMPRA,
                    :O_ST,
                    :O_PRECO_CUSTO;

                SUSPEND;
            END";

            return ExecutarScript(script);
        }



        private IActionResult ExecutarScript(string script) 
        {

             var lojas = _ContextLocal.Empresas
                .Where(x => x.Id < 999)
                .ToList();

        var sucesso = new List<object>();
        var erros = new List<object>();

            foreach (var loja in lojas)
            {
                try
                {
                    using var contextLoja = new ContextControleFiscalContext();
        contextLoja.ConexaoCliente(_ContextLocal, loja.Id);

                    contextLoja.Database.ExecuteSqlRaw(script);

                    sucesso.Add(new
                    {
                        LojaId = loja.Id
    });
                }
                catch (Exception ex)
                {
                    erros.Add(new
                              {
                                  LojaId = loja.Id,
                                  Erro = ex.Message
                              });
                }
            }

            return Ok(new
            {
                TotalLojas = lojas.Count,
                Atualizadas = sucesso.Count,
                ComErro = erros.Count,
                Erros = erros
            });
            }
  
    }
}