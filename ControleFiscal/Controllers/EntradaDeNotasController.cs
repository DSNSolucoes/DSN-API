#nullable disable warnings
using ControleFiscal.Context.NFe;
using ControleFiscal.Infrastructure.Sql;
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Services;
using ControleFiscal.Utils;
using ControleFiscal.Utils.Constant;

using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Xml.Serialization;
using ControleFiscal.Infrastructure.Sql.Focus;
using ControleFiscal.Infrastructure.Sql.Focus.Context;


namespace ControleFiscal.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntradaDeNotasController : ControllerBase
    {
        private readonly ILogger<EntradaDeNotasController> _logger;
        private readonly ContextLocalContext _ContextLocal;
        private readonly ContextControleFiscalContext _ContextEntrada;
        private readonly HttpClient _httpClient;

        public EntradaDeNotasController(ILogger<EntradaDeNotasController> logger, ContextControleFiscalContext ContextEntrada, ContextLocalContext ContextLocal, HttpClient httpClient)
        {
            _logger = logger;
            _ContextLocal = ContextLocal;
            _ContextEntrada = ContextEntrada;
            _httpClient = httpClient;
        }

        private bool SincronizarGrupos(ContextControleFiscalContext context )
        {
            string data = ConfiguracaoHelper.Ler(ConfiguracaoContants.Sincronizador, ConfiguracaoContants.UltimaSincroniaGrupoProdutos);

            if ( data.Trim() == string.Empty)
            {
                data = DateTime.Now.AddYears(-10).ToString();
            }

            var gruposEntrada = _ContextEntrada.GruposProdutos.Where(x => x.Ultimaalteracao > DateTime.Parse(data)).ToList();

            var gruposEmpresas = context.GruposProdutos.ToList();

            foreach (var item in gruposEntrada)
            {
                if (gruposEmpresas.Any(x => x.CdGrupo == item.CdGrupo))
                {
                    GruposProdutos? grupo = gruposEmpresas.FirstOrDefault(x => x.CdGrupo == item.CdGrupo);

                    if (grupo != null)
                    {
                        PreencheGrupos(item, grupo);
                        context.Update(grupo);
                    }
                }
                else
                {
                    context.Add(item);
                }                
            }

            context.SaveChanges();
            ConfiguracaoHelper.Gravar(ConfiguracaoContants.Sincronizador, ConfiguracaoContants.UltimaSincroniaGrupoProdutos, DateTime.Now.ToString());
            return true;
        }

        private bool SincronizarFornecedores(ContextControleFiscalContext context)
        {
            string data = ConfiguracaoHelper.Ler(ConfiguracaoContants.Sincronizador,ConfiguracaoContants.UltimaSincroniaFornecedor);
            
            if (data.Trim() == string.Empty)
            {
                data = DateTime.Now.AddYears(-10).ToString();    
            }

            var fornecedoresEntrada = _ContextEntrada.Fornecedores.Where(x => x.Ultimaalteracao > DateTime.Parse(data)).ToList();

            var fornecedoresEmpresas = context.Fornecedores.ToList();

            foreach (var itemAtualizado in fornecedoresEntrada)
            {
                if (fornecedoresEmpresas.Any(x => x.CdFornecedor == itemAtualizado.CdFornecedor))
                {
                    var fornecedor = fornecedoresEmpresas.FirstOrDefault(x => x.CdFornecedor == itemAtualizado.CdFornecedor);
                    if (fornecedor != null )
                    {
                        EntradaService.AtualizarValores(fornecedor, itemAtualizado);
                        context.Update(fornecedor);
                    }

                }
                else
                {
                    var novo = new Fornecedores();
                    EntradaService.AtualizarValores(novo, itemAtualizado);
                    context.Add(novo);
                }
            }

            context.SaveChanges();
            ConfiguracaoHelper.Gravar(ConfiguracaoContants.Sincronizador,ConfiguracaoContants.UltimaSincroniaFornecedor,DateTime.Now.ToString());
            return true;
        }



        private GruposProdutos PreencheGrupos(GruposProdutos atualizado, GruposProdutos atualizar)
        { 
            atualizar.CdGrupo = atualizado.CdGrupo;
            atualizar.CdPai = atualizado.CdPai;
            atualizar.Descricao = atualizado.Descricao;
            atualizar.DtCadastro = atualizado.DtCadastro;
            atualizar.Ultimaalteracao = atualizado.Ultimaalteracao;
            atualizar.DtSincWeb = atualizado.DtSincWeb;
            atualizar.DtSincSeller = atualizado.DtSincSeller;
            atualizar.Deletado = atualizado.Deletado;
            atualizar.DeletadoDatahora = atualizado.DeletadoDatahora;
            atualizar.DeletadoLogin = atualizado.DeletadoLogin;
            atualizar.CdSetor = atualizado.CdSetor;
            atualizar.DtSincFocus = atualizado.DtSincFocus;
            atualizar.Sincronizando = atualizado.Sincronizando;
            atualizar.UsuarioId = atualizado.UsuarioId;
            atualizar.UsuarioNome = atualizado.UsuarioNome;
            atualizar.PermiteVendaEstZerado = atualizado.PermiteVendaEstZerado;
            return atualizar;
        }

        
        private Clientes AtualizarCliente(Clientes clienteOriginal)
        {

            var novocliente = new Clientes();

            if (clienteOriginal == null  )
            {
                throw new ArgumentNullException("Os objetos de cliente não podem ser nulos.");
            }

            // Obtém o tipo do objeto Cliente
            var tipoCliente = typeof(Clientes);

            // Itera por todas as propriedades da classe Cliente
            foreach (var propriedade in tipoCliente.GetProperties())
            {
                // Obtém o valor da propriedade do objeto clienteAtualizado
                var valorNovo = propriedade.GetValue(novocliente);

                // Define o valor da propriedade no objeto clienteOriginal
                propriedade.SetValue(clienteOriginal, valorNovo);
            }
            return novocliente;
        }

        [HttpPost("ConverterCSVEXCEL")]
        public IActionResult ConverterCSVEXCELNFE([FromForm] IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("Nenhum arquivo enviado.");

            List<NFe> listaNFe = new List<NFe>();

            foreach (var file in files)
            {

                var nfe = new NFe();

                if (file.Length > 0)
                {
                    using (var stream = new StreamReader(file.OpenReadStream()))
                    {
                        // Ler a primeira linha (cabeçalho)
                        var headerLine = stream.ReadLine();
                        if (headerLine == null)
                            return BadRequest("Arquivo vazio.");

                        var headers = headerLine.Split(';'); // Ajuste o delimitador se necessário ("," para CSV padrão)

                        // Criar dicionário para mapear as colunas
                        var columnMap = new Dictionary<string, int>();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            columnMap[headers[i].Trim().ToUpper()] = i;
                        }

                        List<Detalhe> detalhes = new List<Detalhe>();
                        // Ler os dados do CSV
                        while (!stream.EndOfStream)
                        {
                            var item = 0;
                            var linha = stream.ReadLine();
                            if (!string.IsNullOrWhiteSpace(linha))
                            {
                                var valores = linha.Split(';');
                                var cstICMS = columnMap.ContainsKey("CST") ? valores[columnMap["CST"]] : "00";
                                Detalhe det = new Detalhe()
                                {

                                    //Imposto = new Imposto() { IcmsGrupo = preencherICMS(cstICMS) },
                                    nItem = item + 1,
                                    Produto = new Produto()
                                    {
                                        CEST = columnMap.ContainsKey("CEST") ? valores[columnMap["CEST"]] : "",
                                        NCM = columnMap.ContainsKey("NCM") ? valores[columnMap["NCM"]] : "",
                                        CodigoProduto = columnMap.ContainsKey("CODIGO_FORNECEDOR") ? valores[columnMap["CODIGO_FORNECEDOR"]] : "",
                                        Descricao = columnMap.ContainsKey("NOME") ? valores[columnMap["NOME"]] : "00",
                                        EANTributavel = columnMap.ContainsKey("EAN") ? valores[columnMap["EAN"]] : "",
                                        Ean = columnMap.ContainsKey("EAN") ? valores[columnMap["EAN"]] : "",
                                        UnidadeComercial = columnMap.ContainsKey("UND") ? valores[columnMap["UND"]] : "00",
                                        UnidadeTributavel = columnMap.ContainsKey("UND") ? valores[columnMap["UND"]] : "00",
                                        ValorUnitarioComercializacao = columnMap.ContainsKey("VALOR") ? decimal.Parse(valores[columnMap["VALOR"]]) : 0,
                                        ValorUnitarioTributacao = columnMap.ContainsKey("VALOR") ? decimal.Parse(valores[columnMap["VALOR"]]) : 0,
                                        QuantidadeComercial = columnMap.ContainsKey("QTD") ? decimal.Parse(valores[columnMap["QTD"]]) : 0,
                                        QuantidadeTributavel = columnMap.ContainsKey("QTD") ? decimal.Parse(valores[columnMap["QTD"]]) : 0,
                                        ValorBrutoProdutoServico = 0
                                    }

                                };
                                detalhes.Add(det);
                                                                  
                            }

                        }

                        nfe.InformacoesNFe = new NFe.InfNFe() { Detalhe = detalhes };
                    }

                }
                listaNFe.Add(nfe);
            }

            var retorno = SerializeToXml(listaNFe.FirstOrDefault());

            return Ok(retorno);
        }


        [HttpGet("consulta/{cnpj}")]
        public async Task<IActionResult> ConsultarCnpj(string cnpj)
        {
            // Remove caracteres não numéricos
             cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14)
                return BadRequest("CNPJ inválido. Deve conter 14 dígitos.");

            try
            {
                string url = $"https://receitaws.com.br/v1/cnpj/{cnpj}";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Erro ao consultar CNPJ.");

                var json = await response.Content.ReadAsStringAsync();
                var dadosCnpj = JsonConvert.DeserializeObject<DadosCnpj>(json);

                return Ok(dadosCnpj);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }



        public static string SerializeToXml<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, obj);
                return stringWriter.ToString();
            }
        }
        private static IcmsGrupo preencherICMS(string ICMS)
        {
            return new IcmsGrupo() { };
        }

        [HttpGet("ObterDados")]
        public ActionResult<string> ObterEntradas()
        { 
            var Empresas = _ContextLocal.Empresas.Where(x => x.Id == 62).ToList();
            ContextControleFiscalContext contextLoja = new ContextControleFiscalContext();


            try
            {
                foreach (var empresa in Empresas)
                {                    
                    _ContextEntrada.ConexaoCliente(_ContextLocal, 999);
                    contextLoja.ConexaoCliente(_ContextLocal, empresa.Id);

                    SincronizarGrupos( contextLoja);
                    SincronizarFornecedores( contextLoja);

                    //Localizo o cadastro de empresa no servidor de cadastro para encontrar o cd_empresa e filtra a entrada
                    var empresaId = _ContextEntrada.Empresas.FirstOrDefault(x => x.CpfCnpj == empresa.CNPJ).CdEmpresa;

                    var listaEntradas = _ContextEntrada.Entrada.Where(x => x.NotaFechada == "V" &&  x.Sincronizado != "V" && x.Deletado != "V" && x.CdEmpresa == empresaId).ToList();

                    //Pego todas as nota pendentes para este cnpj
                    foreach (var entrada in listaEntradas)
                    {
                        contextLoja.Database.BeginTransaction();
                        _ContextEntrada.Database.BeginTransaction();

                        var entradaItem = _ContextEntrada.EntradaItem.Where(x => x.CdNota == entrada.CdNota && x.Deletado != "V");
                          
                        Entrada entradaLoja = new Entrada();
                        EntradaService.AtualizarValores(entradaLoja, entrada); 
                        entradaLoja.CdNota = 0;
                        contextLoja.Add(entradaLoja);
                        contextLoja.SaveChanges();
                         
                        foreach (var item in entradaItem)
                        {
                            if (item.CodBarras.Length > 12)
                            {
                                var produto = contextLoja?.Produtos?.FirstOrDefault(x => x.CodBarras == item.CodBarras && x.Deletado != "V");

                                produto = produto ?? new Produtos();

                                if (produto is null)//se nao encontrou produto cadastra um novo
                                {
                                    var produtoEntrada = _ContextEntrada.Produtos.FirstOrDefault(x => x.CdProduto == item.CdProduto);

                                    if (produtoEntrada != null)
                                    { 
                                        EntradaService.AtualizarValores(produto!, produtoEntrada);
                                        produto.CdProduto = 0;
                                        contextLoja?.Add(produto);
                                        contextLoja?.SaveChanges();//Tenho que salvar para pegar o retorno cd_produto para rodar a procedure a baixo
                                    }
                                    
                                }

                                EntradaItem entradaItemLoja = new EntradaItem();
                                EntradaService.AtualizarValores(entradaItemLoja, item);

                                entradaItemLoja.CdProduto = produto?.CdProduto;
                                entradaItemLoja.CdNota = entradaLoja.CdNota;
                                contextLoja?.Add(entradaItemLoja);
                                 
                                var produtoId = new FbParameter("@ICD_PRODUTO", produto.CdProduto);//passando o cd_produto do obj produto para garantir que o cd_produto da empresa correto
                                var gradeId = new FbParameter("@ICD_GRADE", 0);
                                var depositoId = new FbParameter("@ICD_DEPOSITO", entrada.CdDeposito); // deposito devera ser igual em todas as Empresas
                                var operacao = new FbParameter("@IOPERACAO", "E");
                                var historico = new FbParameter("@IHISTORICO", "Entrada de Notas Integração");
                                var qunatidade = new FbParameter("@IQUANTIDADE", item.Quantidade);
                                var comprimento = new FbParameter("@IDIM_COMPRIMENTO", 1);
                                var largura = new FbParameter("@IDIM_LARGURA", 1);
                                var espessura = new FbParameter("@IDIM_ESPESSURA", 1);
                                var peso = new FbParameter("@IPESO", 1);
                                var login = new FbParameter("@ILOGIN", "INTEGRACAO");
                                var datahora = new FbParameter("@IDATAHORA", DateTime.Now);
                                var numDocumento = new FbParameter("@INUMDOC", entrada.NumDocumento);
                                var perda = new FbParameter("@IPERDA", 0);

                                var alterado = contextLoja?.Database
                                                       .ExecuteSqlRaw("EXECUTE PROCEDURE SP_ALTERAESTOQUE (@ICD_PRODUTO,@ICD_GRADE,@ICD_DEPOSITO," +
                                                                   "@IOPERACAO,@IHISTORICO,@IQUANTIDADE,@IDIM_COMPRIMENTO,@IDIM_LARGURA,@IDIM_ESPESSURA," +
                                                                   "@IPESO,@ILOGIN,@IDATAHORA,@INUMDOC,@IPERDA)",
                                                                   produtoId, gradeId, depositoId, operacao, historico, qunatidade, comprimento, largura,
                                                                   espessura, peso, login, datahora, numDocumento, perda);

                                item.Sincronizado = "V";
                                _ContextEntrada.Update(item);

                                produto.Precovenda = item.Precovenda;
                                produto.Precocusto = item.Precocusto;
                                produto.Margem = item.Margemlucro;
                                produto.Inativo = "F";

                                contextLoja?.Update(produto);
                            }
                   
                        }

                        var algumErro = entradaItem.Where(x => x.Sincronizado == "F").Count() > 0;

                        if (!algumErro)
                        {
                            entrada.Sincronizado = "V"; 
                            contextLoja?.SaveChanges();

                            _ContextEntrada.Update(entrada);
                            _ContextEntrada.UpdateRange(entradaItem);// para atualizar os item como sincronizado na base de cadastro.
                            _ContextEntrada.SaveChanges();

                            _ContextEntrada.Database.CommitTransaction();
                            contextLoja?.Database.CommitTransaction();
                        }
                        else
                        {
                            contextLoja?.Database.RollbackTransaction();
                            _ContextEntrada.Database.RollbackTransaction();
                        }

                         
                    }
                     


                    //Notas Canceladas.

                    var listaEntradasDeletadas = _ContextEntrada.Entrada.Where(x => x.NotaFechada == "V" && x.Sincronizado != "V" && x.Deletado == "V" && x.CdEmpresa == empresaId).ToList();

                    //Pego todas as nota pendentes para este cnpj
                    foreach (var entrada in listaEntradasDeletadas)
                    {
                        contextLoja.Database.BeginTransaction();
                        _ContextEntrada.Database.BeginTransaction();

                        var entradaItem = _ContextEntrada.EntradaItem.Where(x => x.CdNota == entrada.CdNota && x.Deletado == "V");

                        Entrada entradaLoja = new Entrada();
                        EntradaService.AtualizarValores(entradaLoja, entrada);
                        entradaLoja.CdNota = 0;
                        contextLoja.Add(entradaLoja);
                        contextLoja.SaveChanges();

                        foreach (var item in entradaItem)
                        {
                            if (item.CodBarras.Length > 12)
                            {
                                Produtos produto = contextLoja.Produtos.FirstOrDefault(x => x.CodBarras == item.CodBarras);
                                
                                EntradaItem entradaItemLoja = new EntradaItem();
                                EntradaService.AtualizarValores(entradaItemLoja, item);
                                entradaItemLoja.CdProduto = produto.CdProduto;
                                entradaItemLoja.CdNota = entradaLoja.CdNota;
                                contextLoja.Add(entradaItemLoja);

                                var produtoId = new FbParameter("@ICD_PRODUTO", produto.CdProduto);//passando o cd_produto do obj produto para garantir que o cd_produto da empresa correto
                                var gradeId = new FbParameter("@ICD_GRADE", 0);
                                var depositoId = new FbParameter("@ICD_DEPOSITO", entrada.CdDeposito); // deposito devera ser igual em todas as Empresas
                                var operacao = new FbParameter("@IOPERACAO", "S");
                                var historico = new FbParameter("@IHISTORICO", "Cancelamento Notas Integração");
                                var qunatidade = new FbParameter("@IQUANTIDADE", item.Quantidade);
                                var comprimento = new FbParameter("@IDIM_COMPRIMENTO", 1);
                                var largura = new FbParameter("@IDIM_LARGURA", 1);
                                var espessura = new FbParameter("@IDIM_ESPESSURA", 1);
                                var peso = new FbParameter("@IPESO", 1);
                                var login = new FbParameter("@ILOGIN", "INTEGRACAO");
                                var datahora = new FbParameter("@IDATAHORA", DateTime.Now);
                                var numDocumento = new FbParameter("@INUMDOC", entrada.NumDocumento);
                                var perda = new FbParameter("@IPERDA", 0);

                                var alterado = contextLoja.Database
                                                       .ExecuteSqlRaw("EXECUTE PROCEDURE SP_ALTERAESTOQUE (@ICD_PRODUTO,@ICD_GRADE,@ICD_DEPOSITO," +
                                                                   "@IOPERACAO,@IHISTORICO,@IQUANTIDADE,@IDIM_COMPRIMENTO,@IDIM_LARGURA,@IDIM_ESPESSURA," +
                                                                   "@IPESO,@ILOGIN,@IDATAHORA,@INUMDOC,@IPERDA)",
                                                                   produtoId, gradeId, depositoId, operacao, historico, qunatidade, comprimento, largura,
                                                                   espessura, peso, login, datahora, numDocumento, perda);

                                item.Sincronizado = "V";
                                _ContextEntrada.Update(item);


                            }

                        }

                        var algumErro = entradaItem.Where(x => x.Sincronizado == "F").Count() > 0;

                        if (!algumErro)
                        {
                            entrada.Sincronizado = "V";
                            contextLoja.SaveChanges();

                            _ContextEntrada.Update(entrada);
                            _ContextEntrada.UpdateRange(entradaItem);// para atualizar os item como sincronizado na base de cadastro.
                            _ContextEntrada.SaveChanges();

                            _ContextEntrada.Database.CommitTransaction();
                            contextLoja.Database.CommitTransaction();
                        }
                        else
                        {
                            contextLoja.Database.RollbackTransaction();
                            _ContextEntrada.Database.RollbackTransaction();
                        } 
                    }

                }

                return ("OK");
            }
            catch (Exception e)
            {
                return BadRequest(e);
                throw;
            }
        }
        private Produtos PreencherProdutoNovo(Produtos novoEntrada)
        {
            Produtos novoCadastro = new Produtos();


            novoCadastro.NmProduto = novoEntrada.NmProduto;
            novoCadastro.CodBarras = novoEntrada.CodBarras;
            novoCadastro.CdGrupo = novoEntrada.CdGrupo;
            novoCadastro.Unidade = novoEntrada.Unidade;
            novoCadastro.CxCodBarraCaixa = novoEntrada.CxCodBarraCaixa;
            novoCadastro.CxUndconversao = novoEntrada.CxUndconversao;
            novoCadastro.Pesoliquido = novoEntrada.Pesoliquido;
            novoCadastro.Pesobruto = novoEntrada.Pesobruto;
            novoCadastro.Descontomaximo = novoEntrada.Descontomaximo;
            novoCadastro.Comissao = novoEntrada.Comissao;
            novoCadastro.BalCodBalanca = novoEntrada.BalCodBalanca;
            novoCadastro.BalGerarcodbalanca = novoEntrada.BalGerarcodbalanca;
            novoCadastro.Localizacao = novoEntrada.Localizacao;
            novoCadastro.Classfiscal = novoEntrada.Classfiscal;
            novoCadastro.Ipi = novoEntrada.Ipi;
            novoCadastro.Precocusto = novoEntrada.Precocusto;
            novoCadastro.Margem = novoEntrada.Margem;
            novoCadastro.Precovenda = novoEntrada.Precovenda;
            novoCadastro.DtUltimoreajuste = novoEntrada.DtUltimoreajuste;
            novoCadastro.Precovendaanterior = novoEntrada.Precovendaanterior;
            novoCadastro.Precocustoanterior = novoEntrada.Precocustoanterior;
            novoCadastro.PdvPrecovenda = novoEntrada.PdvPrecovenda;
            novoCadastro.Estoqueminimo = novoEntrada.Estoqueminimo;
            novoCadastro.Inativo = novoEntrada.Inativo;
            novoCadastro.Obs = novoEntrada.Obs;
            novoCadastro.Foto = novoEntrada.Foto;
            novoCadastro.CdSimilar = novoEntrada.CdSimilar;
            novoCadastro.DtCadastro = novoEntrada.DtCadastro;
            novoCadastro.Ultimaalteracao = novoEntrada.Ultimaalteracao;
            novoCadastro.Fabricante = novoEntrada.Fabricante;
            novoCadastro.Aplicacao = novoEntrada.Aplicacao;
            novoCadastro.Trescasas = novoEntrada.Trescasas;
            novoCadastro.Dimensoes = novoEntrada.Dimensoes;
            novoCadastro.Quanttroca = novoEntrada.Quanttroca;
            novoCadastro.Exportar = novoEntrada.Exportar;
            novoCadastro.Precopropeso = novoEntrada.Precopropeso;
            novoCadastro.DescricaoAbrev = novoEntrada.DescricaoAbrev;
            novoCadastro.Codigo02 = novoEntrada.Codigo02;
            novoCadastro.Codigo03 = novoEntrada.Codigo03;
            novoCadastro.Codigo04 = novoEntrada.Codigo04;
            novoCadastro.Customedio = novoEntrada.Customedio;
            novoCadastro.Tipocusto = novoEntrada.Tipocusto;
            novoCadastro.Ultimocusto = novoEntrada.Ultimocusto;
            novoCadastro.UndFracionada = novoEntrada.UndFracionada;
            novoCadastro.Undconversaoentrada = novoEntrada.Undconversaoentrada;
            novoCadastro.Margem2 = novoEntrada.Margem2;
            novoCadastro.Margem3 = novoEntrada.Margem3;
            novoCadastro.Margem4 = novoEntrada.Margem4;
            novoCadastro.Margem5 = novoEntrada.Margem5;
            novoCadastro.DtExpPalm = novoEntrada.DtExpPalm;
            novoCadastro.Preco2 = novoEntrada.Preco2;
            novoCadastro.Preco3 = novoEntrada.Preco3;
            novoCadastro.Preco4 = novoEntrada.Preco4;
            novoCadastro.Preco5 = novoEntrada.Preco5;
            novoCadastro.Controlanumserie = novoEntrada.Controlanumserie;
            novoCadastro.RefPromocao = novoEntrada.RefPromocao;
            novoCadastro.DtSincWeb = novoEntrada.DtSincWeb;
            novoCadastro.Icms = novoEntrada.Icms;
            novoCadastro.Fretechegada = novoEntrada.Fretechegada;
            novoCadastro.Fretesaida = novoEntrada.Fretesaida;
            novoCadastro.DtSincSeller = novoEntrada.DtSincSeller;
            novoCadastro.CdEmpresa = novoEntrada.CdEmpresa;
            novoCadastro.Origem = novoEntrada.Origem;
            novoCadastro.Substtributaria = novoEntrada.Substtributaria;
            novoCadastro.ItemAbatedouro = novoEntrada.ItemAbatedouro;
            novoCadastro.Deletado = novoEntrada.Deletado;
            novoCadastro.DtExpWeb = novoEntrada.DtExpWeb;
            novoCadastro.Iat = novoEntrada.Iat;
            novoCadastro.Ippt = novoEntrada.Ippt;
            novoCadastro.Ultimoipi = novoEntrada.Ultimoipi;
            novoCadastro.Ultimoicmsst = novoEntrada.Ultimoicmsst;
            novoCadastro.Ultimooutros = novoEntrada.Ultimooutros;
            novoCadastro.Ultimofrete = novoEntrada.Ultimofrete;
            novoCadastro.Ultimoseguro = novoEntrada.Ultimoseguro;
            novoCadastro.OfertaObrigatoria = novoEntrada.OfertaObrigatoria;
            novoCadastro.Extipi = novoEntrada.Extipi;
            novoCadastro.Cst = novoEntrada.Cst;
            novoCadastro.PossuiDimensoes = novoEntrada.PossuiDimensoes;
            novoCadastro.DeletadoDatahora = novoEntrada.DeletadoDatahora;
            novoCadastro.DeletadoLogin = novoEntrada.DeletadoLogin;
            novoCadastro.DtSincFocus = novoEntrada.DtSincFocus;
            novoCadastro.Sincronizando = novoEntrada.Sincronizando;
            novoCadastro.Pis = novoEntrada.Pis;
            novoCadastro.PisCst = novoEntrada.PisCst;
            novoCadastro.Cofins = novoEntrada.Cofins;
            novoCadastro.CofinsCst = novoEntrada.CofinsCst;
            novoCadastro.CestaBasica = novoEntrada.CestaBasica;
            novoCadastro.IpiCst = novoEntrada.IpiCst;
            novoCadastro.CodFornecedor = novoEntrada.CodFornecedor;
            novoCadastro.ECombustivel = novoEntrada.ECombustivel;
            novoCadastro.NaoVende = novoEntrada.NaoVende;
            novoCadastro.Preco6 = novoEntrada.Preco6;
            novoCadastro.Preco7 = novoEntrada.Preco7;
            novoCadastro.Preco8 = novoEntrada.Preco8;
            novoCadastro.Preco9 = novoEntrada.Preco9;
            novoCadastro.Preco10 = novoEntrada.Preco10;
            novoCadastro.Preco11 = novoEntrada.Preco11;
            novoCadastro.Margem6 = novoEntrada.Margem6;
            novoCadastro.Margem7 = novoEntrada.Margem7;
            novoCadastro.Margem8 = novoEntrada.Margem8;
            novoCadastro.Margem9 = novoEntrada.Margem9;
            novoCadastro.Margem10 = novoEntrada.Margem10;
            novoCadastro.Margem11 = novoEntrada.Margem11;
            novoCadastro.AliquotaEcf = novoEntrada.AliquotaEcf;
            novoCadastro.LoginUltimaalteracao = novoEntrada.LoginUltimaalteracao;
            novoCadastro.Cest = novoEntrada.Cest;
            novoCadastro.FreteValor = novoEntrada.FreteValor;
            novoCadastro.EmDestaque = novoEntrada.EmDestaque;
            novoCadastro.EngradadoQtde = novoEntrada.EngradadoQtde;
            novoCadastro.CdEngradado = novoEntrada.CdEngradado;
            novoCadastro.CdModelo = novoEntrada.CdModelo;
            novoCadastro.DiasValidade = novoEntrada.DiasValidade;
            novoCadastro.CdProdEstoqueCompartilhado = novoEntrada.CdProdEstoqueCompartilhado;
            novoCadastro.Usaestoquecompartilhado = novoEntrada.Usaestoquecompartilhado;
            novoCadastro.PorcPerdaEstoque = novoEntrada.PorcPerdaEstoque;
            novoCadastro.DestaqueAutomatico = novoEntrada.DestaqueAutomatico;
            novoCadastro.WebId = novoEntrada.WebId;
            novoCadastro.VariacaoId = novoEntrada.VariacaoId;
            novoCadastro.IdPromocaokit = novoEntrada.IdPromocaokit;
            novoCadastro.MarcaId = novoEntrada.MarcaId;
            novoCadastro.Descontofixo = novoEntrada.Descontofixo;
            novoCadastro.ObsInterna = novoEntrada.ObsInterna;
            novoCadastro.CodBeneficioFiscal = novoEntrada.CodBeneficioFiscal;
            novoCadastro.MotivoDesoneracaoIcms = novoEntrada.MotivoDesoneracaoIcms;
            novoCadastro.ControlaLote = novoEntrada.ControlaLote;
            novoCadastro.CombCodanp = novoEntrada.CombCodanp;
            novoCadastro.CombDescanp = novoEntrada.CombDescanp;
            novoCadastro.CombCodif = novoEntrada.CombCodif;
            novoCadastro.CombUfConsumo = novoEntrada.CombUfConsumo;
            novoCadastro.CombPglp = novoEntrada.CombPglp;
            novoCadastro.CombPgnn = novoEntrada.CombPgnn;
            novoCadastro.CombPgni = novoEntrada.CombPgni;
            novoCadastro.CombVpart = novoEntrada.CombVpart;
            novoCadastro.CombQtfattempamb = novoEntrada.CombQtfattempamb;
            novoCadastro.CombBccide = novoEntrada.CombBccide;
            novoCadastro.CombAliqcide = novoEntrada.CombAliqcide;
            novoCadastro.CombValcide = novoEntrada.CombValcide;
            novoCadastro.CombEncNbico = novoEntrada.CombEncNbico;
            novoCadastro.CombEncNbomba = novoEntrada.CombEncNbomba;
            novoCadastro.CombEncNtanque = novoEntrada.CombEncNtanque;
            novoCadastro.CombEncVencini = novoEntrada.CombEncVencini;
            novoCadastro.CombEncVencfin = novoEntrada.CombEncVencfin;
            novoCadastro.EMedicamento = novoEntrada.EMedicamento;
            novoCadastro.IcmsAliquotaCalcDesoneracao = novoEntrada.IcmsAliquotaCalcDesoneracao;
            novoCadastro.ReducaoBaseCalculo = novoEntrada.ReducaoBaseCalculo;
            novoCadastro.Descfixopreco2 = novoEntrada.Descfixopreco2;
            novoCadastro.Descfixopreco3 = novoEntrada.Descfixopreco3;
            novoCadastro.Descfixopreco4 = novoEntrada.Descfixopreco4;
            novoCadastro.Descfixopreco5 = novoEntrada.Descfixopreco5;
            novoCadastro.Descfixopreco6 = novoEntrada.Descfixopreco6;
            novoCadastro.Descfixopreco7 = novoEntrada.Descfixopreco7;
            novoCadastro.Descfixopreco8 = novoEntrada.Descfixopreco8;
            novoCadastro.Descfixopreco9 = novoEntrada.Descfixopreco9;
            novoCadastro.Descfixopreco10 = novoEntrada.Descfixopreco10;
            novoCadastro.Descfixopreco11 = novoEntrada.Descfixopreco11;
            novoCadastro.Descfixomargem2 = novoEntrada.Descfixomargem2;
            novoCadastro.Descfixomargem3 = novoEntrada.Descfixomargem3;
            novoCadastro.Descfixomargem4 = novoEntrada.Descfixomargem4;
            novoCadastro.Descfixomargem5 = novoEntrada.Descfixomargem5;
            novoCadastro.Descfixomargem6 = novoEntrada.Descfixomargem6;
            novoCadastro.Descfixomargem7 = novoEntrada.Descfixomargem7;
            novoCadastro.Descfixomargem8 = novoEntrada.Descfixomargem8;
            novoCadastro.Descfixomargem9 = novoEntrada.Descfixomargem9;
            novoCadastro.Descfixomargem10 = novoEntrada.Descfixomargem10;
            novoCadastro.Descfixomargem11 = novoEntrada.Descfixomargem11;
            novoCadastro.IntegraMarketplace = novoEntrada.IntegraMarketplace;
            novoCadastro.Customanual = novoEntrada.Customanual;
            novoCadastro.Adremicmsret = novoEntrada.Adremicmsret;
            novoCadastro.Qbcmonoret = novoEntrada.Qbcmonoret;
            novoCadastro.Custocompra = novoEntrada.Custocompra;

            return novoCadastro;
        }

    }
}
