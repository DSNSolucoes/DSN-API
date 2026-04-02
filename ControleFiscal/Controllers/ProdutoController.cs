using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Utils;
using ControleFiscal.Infrastructure.Sql.Focus; 

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Domain.DTO.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProdutoController : ControllerBase
    {

        private readonly ILogger<ProdutoController> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public ProdutoController(ILogger<ProdutoController> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        [HttpGet("Obter")]
        public ActionResult<List<ProdutosDTO>> Obter([FromQuery] List<int> lojaId, [FromQuery] int empresaFornecedor = 0, [FromQuery] string? pesquisa = "", [FromQuery] int? grupoId = 0, [FromQuery] int? fornecedorId = 0, [FromQuery] int ordenacao = 0)
        {
            var listaRetorno = new List<ProdutosDTO>();
            try
            {
                if (empresaFornecedor > 0)
                {
                    _Context.ConexaoCliente(empresaFornecedor, _ContextLocal);
                }

                string fornecedorCNPJ = "";
                if (fornecedorId is not null && fornecedorId > 0)
                {
                    fornecedorCNPJ = _Context.Fornecedores.First(x => x.CdFornecedor == fornecedorId).CpfCnpj;
                }

                var lojas = _ContextLocal.Lojas.Where(x => lojaId.Contains(x.Id) && x.Id < 999);//Para nao pegar nada do Entrada
                var nomeLoja = string.Empty;

                foreach (var item in lojas)
                {
                    try
                    {
                        ContextControleFiscalContext contextLoja = new ContextControleFiscalContext();
                        nomeLoja = contextLoja.ConexaoCliente(item.Id, _ContextLocal);

                        if (fornecedorId is not null && fornecedorId > 0)
                        {
                            var fornec = contextLoja?.Fornecedores?.FirstOrDefault(x => x.CpfCnpj == fornecedorCNPJ);
                            fornecedorId = fornec?.CdFornecedor;
                        }


                        var joinGrupo = " left join sp_pertence_ao_grupo(p.cd_produto,:d) pertence on 1 = 1 ";
                        var joinFornecedor = " left join produtos_ref ref on ref.cd_produto = p.cd_produto ";

                        var sql = "select First(100) p.* from  produtos p " + (grupoId > 0 ? joinGrupo : "") + (fornecedorId > 0 ? joinFornecedor : "") + " where p.deletado <>  'V' and p.inativo <> 'V' ";

                        if (!string.IsNullOrEmpty(pesquisa))
                        {
                            List<string> listaFiltro = pesquisa.Split(' ').ToList();

                            sql = sql + " and ( ";

                            foreach (string filtro in listaFiltro)
                            {
                                sql = sql + " (nm_produto like  '%" + filtro + "%') and ";

                                if (filtro.All(Char.IsDigit))
                                {
                                    sql = sql + " (cod_barras like  '%" + filtro + "%') and ";
                                }
                            }
                            sql = sql.Substring(0, sql.Length - 4) + ')';
                        }

                        if (grupoId > 0)
                        {
                            sql = sql + " coalesce(pertence,'F') = 'V' ";
                        }

                        listaRetorno.AddRange(contextLoja.Produtos.FromSqlRaw(sql).ToList().Select(x => new ProdutosDTO()
                        {
                            CdProduto = x.CdProduto,
                            CodBarras = x.CodBarras,
                            Margem = x.Margem,
                            NmProduto = x.NmProduto,
                            NomeLoja = nomeLoja,
                            PdvPrecovenda = x.PdvPrecovenda,
                            Precocusto = x.Precocusto,
                            Precovenda = x.Precovenda
                        }));
                    }
                    catch (Exception e)
                    {
                        listaRetorno.Add(new ProdutosDTO() { NomeLoja = nomeLoja, Erro = e.StackTrace });

                    }

                }

                switch (ordenacao)
                {
                    case 1:
                        return Ok(listaRetorno.OrderBy(x => x.NmProduto));
                        

                    default:
                        return Ok(listaRetorno.OrderBy(x => x.CodBarras));
                       
                }
                 

            }
            catch (Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }


        [HttpGet("ObterGruposProdutos")]
        public ActionResult<List<ComboDTO>> ObterGrupos([FromQuery] int lojaId)
        {
            string? nomeloja = "";
            try
            {
                var loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);
                nomeloja = loja?.Nome;

                _Context.ConexaoCliente(loja?.Caminho, loja?.Host); ;

                //var lista = _Context.GruposProdutos.Where(x => x.Deletado != "V").ToList().Select(x => new ComboDTO { Id = x.CdGrupo, Descricao = x.Descricao} );

                StringBuilder sql = new StringBuilder();

                sql.Append("select gp.cd_grupo as ID, NC.string as descricao from grupos_produtos gp left join sp_string_grupo(gp.cd_grupo) NC on  1=1 where coalesce(gp.deletado,'F') <> 'V' order by 2");
                var lista = _Context.GruposProdutosDTO.FromSqlRaw(@sql.ToString()).ToList();


                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeloja));
            }

        }



        [HttpGet("ObterDeletados")]
        public ActionResult<List<Produtos>> ObterDeletados([FromQuery] int lojaId)
        {
            var nomeloja = "";
            try
            {
                var loja = _ContextLocal.Lojas.FirstOrDefault(x => x.Id == lojaId);
                nomeloja = loja?.Nome;

                _Context.ConexaoCliente(loja?.Caminho, loja?.Host); ;

                var lista = _Context.Produtos.Where(x => x.Deletado == "V").Take(100).ToList();


                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest(TratarExeption.TratarMensagemUsuario(e, nomeloja));
            }

        }


        [HttpGet("ObterRelatorioProdutosLojas")]
        public IActionResult ObterRelatorioProdutosLojas()
        {
            var lojas = _ContextLocal.Lojas.Where(x => x.Id < 200).ToList();

            var resultado = new List<RelatorioProdutosLojasDTO>();

            foreach (var item in lojas)
            {
                try
                {
                    _Context.ConexaoCliente(_ContextLocal, item.Id);
                    var produtos = _Context.Produtos.Where(x => x.Deletado != "V" && x.Inativo != "V").Select(x => new RelatorioProdutosLojasDTO
                    {
                        CodBarras = x.CodBarras,
                        Descricao = x.NmProduto,
                        NomeLoja = item.Nome,
                        PrecoCusto = x.Precocusto ?? 0,
                        PrecoVenda = x.PdvPrecovenda ?? 0,
                        EstoqueAtual = _Context.Estoque.FirstOrDefault(x => x.CdProduto == x.CdProduto)!.Estoqueatual ?? 0
                    }).ToList();

                    resultado.AddRange(produtos);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            StringBuilder relatorio = new StringBuilder();

            relatorio.AppendLine("Loja;Cód de Barras;Nome Produto;PrecoCusto;PrecoVenda;Estoque");

            foreach (var item in resultado)
            {
                relatorio.Append($"\"{item.NomeLoja}\";");
                relatorio.Append($"\"{item.CodBarras}\";");
                relatorio.Append($"\"{item.Descricao}\";");
                relatorio.Append($"\"{item.PrecoCusto}\";");
                relatorio.Append($"\"{item.PrecoVenda}\";");
                relatorio.Append($"\"{item.EstoqueAtual}\";");
                relatorio.AppendLine("");
            }


            string nomeArquivo = $"Relatorio_Produtos_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";
            byte[] bytes = Encoding.UTF8.GetBytes(relatorio.ToString());
            bytes = Encoding.UTF8.GetPreamble().Concat(bytes).ToArray();
            return File(bytes, "text/csv", nomeArquivo);

        }
    }
}