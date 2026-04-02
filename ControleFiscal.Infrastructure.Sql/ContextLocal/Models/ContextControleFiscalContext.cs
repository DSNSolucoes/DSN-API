 
using Microsoft.EntityFrameworkCore;

namespace ControleFiscal.Infrastructure.Sql.Focus.Context
{
    public partial class ContextControleFiscalContext : DbContext
    {
        public ContextControleFiscalContext()
        {
        }

        public ContextControleFiscalContext(DbContextOptions<ContextControleFiscalContext> options)
            : base(options)
        {
        }

        #region DbSet
        public virtual DbSet<Caixa> Caixa { get; set; }
        public virtual DbSet<CaixaEventos> CaixaEventos { get; set; }
        public virtual DbSet<CaixaSituacao> CaixaSituacao { get; set; }
        public virtual DbSet<CestNcm> CestNcm { get; set; }
        public virtual DbSet<ClienteCategoriasFormpag> ClienteCategoriasFormpag { get; set; }
        public virtual DbSet<ClienteComputador> ClienteComputador { get; set; }
        public virtual DbSet<ClienteComputadorDisco> ClienteComputadorDisco { get; set; }
        public virtual DbSet<ClienteComputadorSistema> ClienteComputadorSistema { get; set; }
        public virtual DbSet<ClienteComputadorSistemaModu> ClienteComputadorSistemaModu { get; set; }
        public virtual DbSet<ClienteContabancaria> ClienteContabancaria { get; set; }
        public virtual DbSet<ClienteGrupos> ClienteGrupos { get; set; }
        public virtual DbSet<ClienteHistorico> ClienteHistorico { get; set; }
        public virtual DbSet<ClienteUsuario> ClienteUsuario { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<ClientesRef> ClientesRef { get; set; }
        public virtual DbSet<ConfigCadastro> ConfigCadastro { get; set; }
        public virtual DbSet<ConfigEmpresaConsulta> ConfigEmpresaConsulta { get; set; }
        public virtual DbSet<ConfigLocal> ConfigLocal { get; set; }
        public virtual DbSet<ConfigPedido> ConfigPedido { get; set; }
        public virtual DbSet<ConfigSincronizacao> ConfigSincronizacao { get; set; }
        public virtual DbSet<ConfigSped> ConfigSped { get; set; }
        public virtual DbSet<Confimpdoc> Confimpdoc { get; set; }
        public virtual DbSet<DfeSerieNumeracao> DfeSerieNumeracao { get; set; }
        public virtual DbSet<Empresas> Empresas { get; set; }
        public virtual DbSet<EndCertificadoConfig> EndCertificadoConfig { get; set; }
        public virtual DbSet<Entrada> Entrada { get; set; }
        public virtual DbSet<EntradaConfig> EntradaConfig { get; set; }
        public virtual DbSet<EntradaFiscal> EntradaFiscal { get; set; }
        public virtual DbSet<EntradaGrade> EntradaGrade { get; set; }
        public virtual DbSet<EntradaGradeQuantidade> EntradaGradeQuantidade { get; set; }
        public virtual DbSet<EntradaItem> EntradaItem { get; set; }
        public virtual DbSet<EntradaItemFiscal> EntradaItemFiscal { get; set; }
        public virtual DbSet<EntradaItemImportLacres> EntradaItemImportLacres { get; set; }
        public virtual DbSet<EntradaItemImportacao> EntradaItemImportacao { get; set; }
        public virtual DbSet<EntradaItemMedicamento> EntradaItemMedicamento { get; set; }
        public virtual DbSet<EntradaNumserie> EntradaNumserie { get; set; }
        public virtual DbSet<Estoque> Estoque { get; set; }
        public virtual DbSet<EstoqueAlteracaomanual> EstoqueAlteracaomanual { get; set; }
        public virtual DbSet<EstoqueData> EstoqueData { get; set; }
        public virtual DbSet<Fornecedores> Fornecedores { get; set; }
        public virtual DbSet<FotosTransicao> FotosTransicao { get; set; }
        public virtual DbSet<Grades> Grades { get; set; }
        public virtual DbSet<Grids> Grids { get; set; }
        public virtual DbSet<GruposProdutos> GruposProdutos { get; set; }
        public virtual DbSet<GruposPromocionais> GruposPromocionais { get; set; }
        public virtual DbSet<NfceCancelada> NfceCancelada { get; set; }
        public virtual DbSet<NfceCanceladaXml> NfceCanceladaXml { get; set; }
        public virtual DbSet<NfceConfig> NfceConfig { get; set; }
        public virtual DbSet<NfceConfigLocal> NfceConfigLocal { get; set; }
        public virtual DbSet<NfceContingController> NfceContingController { get; set; }
        public virtual DbSet<NfceInutilizada> NfceInutilizada { get; set; }
        public virtual DbSet<NfceInutilizadaXml> NfceInutilizadaXml { get; set; }
        public virtual DbSet<NfcePedido> NfcePedido { get; set; }
        public virtual DbSet<NfcePedidoItem> NfcePedidoItem { get; set; }
        public virtual DbSet<NfceXml> NfceXml { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<PedidoCategoria> PedidoCategoria { get; set; }
        public virtual DbSet<PedidoCompraPagamento> PedidoCompraPagamento { get; set; }
        public virtual DbSet<PedidoItem> PedidoItem { get; set; }
        public virtual DbSet<PedidoItemNumserie> PedidoItemNumserie { get; set; }
        public virtual DbSet<PedidoPagamento> PedidoPagamento { get; set; }
        public virtual DbSet<Prodfornec> Prodfornec { get; set; }
        public virtual DbSet<ProdutoSimilar> ProdutoSimilar { get; set; }
        public virtual DbSet<Produtos> Produtos { get; set; }
        public virtual DbSet<ProdutosFoto> ProdutosFoto { get; set; }
        public virtual DbSet<ProdutosMarcas> ProdutosMarcas { get; set; }
        public virtual DbSet<ProdutosMarketplace> ProdutosMarketplace { get; set; }
        public virtual DbSet<ProdutosReajustes> ProdutosReajustes { get; set; }
        public virtual DbSet<ProdutosRef> ProdutosRef { get; set; }
        public virtual DbSet<ProdutosTributacao> ProdutosTributacao { get; set; }
        public virtual DbSet<ProdutosUrl> ProdutosUrl { get; set; }
        public virtual DbSet<Terminais> Terminais { get; set; }
        public virtual DbSet<TerminaisLigados> TerminaisLigados { get; set; }
        public virtual DbSet<TerminaisRef> TerminaisRef { get; set; }
        public virtual DbSet<Textos> Textos { get; set; }
        public virtual DbSet<Tipopreco> Tipopreco { get; set; }
        public virtual DbSet<Unidade> Unidade { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }
        public virtual DbSet<UsuariosCategoria> UsuariosCategoria { get; set; }
        public virtual DbSet<UsuariosRef> UsuariosRef { get; set; }
        #endregion



        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
