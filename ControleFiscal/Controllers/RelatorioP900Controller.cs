using ControleFiscal.Domain.DTO.Relatorio;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Utils; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using System.Reflection;
using System.Text;
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Infrastructure.Sql.Focus.Context;
namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelatorioP900Controller : ControllerBase
    {

        private readonly ILogger<RelatorioP900Controller> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public RelatorioP900Controller(ILogger<RelatorioP900Controller> logger, ContextControleFiscalContext Context, ContextLocalContext ContextoLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextoLocal;
        }

        [HttpGet]
        public ActionResult Obter([FromQuery] int lojaId,  [FromQuery] int? fornecedorId,    [FromQuery] int? itensPorPagina, [FromQuery] int pagina, [FromQuery] string? ordenarDirecao, [FromQuery] string? ordenarPor)
        {
            try
            {
                var retorno = ObterDados(lojaId,  fornecedorId, ordenarPor, ordenarDirecao);
                
                RetornoPaginado<RelatorioP900DTO> retornoPaginado = new RetornoPaginado<RelatorioP900DTO>();
                retornoPaginado.QtdRegistro = retorno.Count;
                var itens = itensPorPagina != null && itensPorPagina > 0 ? (pagina - 1) * itensPorPagina.Value : 0;
                retornoPaginado.Lista = retorno.Skip(itens).Take(itensPorPagina.GetValueOrDefault(20)).ToList();

                return Ok(retornoPaginado);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }


        }


        private List<RelatorioP900DTO> ObterDados(int lojaId,int? fornecedorId, string? ordenarPor,string? ordenarDirecao)
        {
            try
            {
                var nomeLoja = _Context.ConexaoCliente(lojaId, _ContextLocal);

                StringBuilder sql = new StringBuilder();
                //Agruapado

                sql.Append("select p.cod_barras as codBarras,p.cod_fornecedor as codReferencia, p.nm_produto as descricao,f.nm_fornecedor as fornecedor,f.cd_fornecedor as fornecedorId, ");
                sql.Append("estoque.estoqueatual as estoque  ");
                sql.Append("from produtos p  left join produtos_ref pr on pr.cd_produto = p.cd_produto ");
                sql.Append("left join fornecedores f on f.cd_fornecedor = pr.ultimofornecedor ");
                sql.Append("left join sp_pegaestoque(p.cd_produto, 0, 0) estoque on 1 = 1 ");
                sql.Append("where  coalesce(p.deletado,'V') <> 'V' and coalesce(p.inativo,'V') <> 'V' ");

                if (fornecedorId != null && fornecedorId > 0) {
                    sql.Append(" and  f.cd_fornecedor = " + fornecedorId);
                } 
                  
                var retorno = _Context.RelatoriosP900.FromSqlRaw(@sql.ToString()).ToList(); 
                retorno = AplicarOrdenacao(retorno,  ordenarPor, ordenarDirecao);

                return retorno; 

            }
            catch (Exception)
            {
                return  new List<RelatorioP900DTO>();
            }


        }


        private static List<RelatorioP900DTO> AplicarOrdenacao(List<RelatorioP900DTO> lista, string? ordenarPor, string? ordenarDirecao)
        {
            if (lista == null || lista.Count == 0)
                return new List<RelatorioP900DTO>();

            if (string.IsNullOrWhiteSpace(ordenarPor) || string.IsNullOrWhiteSpace(ordenarDirecao))
                return lista;
             
            var campo = StringUtils.NormalizePropName(ordenarPor);

            var prop = typeof(RelatorioP900DTO).GetProperty(
                campo,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
            );

            if (prop == null)
                return lista;

            bool desc = string.Equals(ordenarDirecao, "desc", StringComparison.OrdinalIgnoreCase);

            // Ordenação genérica (lida com null)
            return desc
                ? lista.OrderByDescending(x => prop.GetValue(x, null) ?? string.Empty).ToList()
                : lista.OrderBy(x => prop.GetValue(x, null) ?? string.Empty).ToList();
        }

   
         

        [HttpGet("exportar")]
        public IActionResult ExportarRelatorio([FromQuery] int lojaId, [FromQuery] int? fornecedorId, [FromQuery] string? ordenarDirecao, [FromQuery] string? ordenarPor)
        {
            try
            {
                var resultado = ObterDados(lojaId,   fornecedorId, ordenarPor, ordenarDirecao);  

                StringBuilder relatorio = new StringBuilder(); 

                relatorio.AppendLine("Código de barras; Código de referência; Descrição; Nome do fornecedor;Quantidade");

                foreach (var item in resultado)
                {
                    relatorio.Append($"\"{item.CodBarras}\";");
                    relatorio.Append($"\"{item.CodReferencia}\";");
                    relatorio.Append($"\"{item.Descricao}\";");
                    relatorio.Append($"\"{item.Fornecedor}\";");
                    relatorio.Append($"\"{item.Estoque}\";");
               
                    relatorio.AppendLine("");
                }
                 

                string nomeArquivo = $"Relatorio_P900_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
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