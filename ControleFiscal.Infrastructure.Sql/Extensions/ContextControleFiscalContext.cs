using Microsoft.EntityFrameworkCore;
using ControleFiscal.Domain.DTO.Relatorio;

using ControleFiscal.Infrastructure.Sql.Focus.Configurations;
using ControleFiscal.Domain.DTO.Focus;
using ControleFiscal.Domain.DTO.ControleFiscal;


namespace ControleFiscal.Infrastructure.Sql.Focus.Context
{ 
    public partial class ContextControleFiscalContext : DbContext
    {
        private string _clienteId;
        private string _host;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { 
            string conn = "User=SYSDBA;Password=masterkey;Database=" + _clienteId + ";DataSource=" + _host + ";Port=3050;Dialect=3;Charset=UTF8;";
            optionsBuilder.UseFirebird(conn, o => o.WithExplicitStringLiteralTypes()).AddInterceptors(new SqlInterceptor()); 
            optionsBuilder.LogTo(Console.Write);             
                    
        }

        public string ConexaoCliente(int lojaId, ContextLocalContext _ContextLocal)
        {
            try
            {
                var loja = _ContextLocal.Empresas.FirstOrDefault(x => x.Id == lojaId);

                _clienteId = loja.Caminho;
                _host = loja.Host;

                var context = new DataContext(_clienteId, _host).CreateDbContext();


                return loja.Nome;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public DbContext ConexaoCliente(ContextLocalContext _ContextLocal, int lojaId)
        {
            var loja = _ContextLocal.Empresas.FirstOrDefault(x => x.Id == lojaId);

            _clienteId = loja.Caminho;
            _host = loja.Host;

            return new DataContext(_clienteId, _host).CreateDbContext();
        }

        public DbContext ConexaoCliente(string caminho, string phost)
        {

            _clienteId = caminho;
            _host = phost;

            var context = new DataContext(_clienteId, _host).CreateDbContext();


            return context;
        }


        #region DBSet
        public virtual DbSet<RelatorioV50DTO> RelatoriosV50 { get; set; }
        public virtual DbSet<ProdutoDTOVinculo> AtualizarVinculo { get; set; }
        public virtual DbSet<EntradaNotasSelecaoDTO> EntradaNotasSelecao { get; set; }
        public virtual DbSet<RelatorioE01DTO> RelatoriosE01 { get; set; }
        public virtual DbSet<RelatorioE03DTO> RelatoriosE03 { get; set; }
        public virtual DbSet<RelatorioF04DTO> RelatoriosF04 { get; set; }
        public virtual DbSet<RelatorioP900DTO> RelatoriosP900 { get; set; }
        public virtual DbSet<RelatorioF04DTOAgrupado> RelatoriosF04Agrupado { get; set; }
        public virtual DbSet<AlteraEstoqueDTO> AlterarEstoque { get; set; }
        public virtual DbSet<ComboDTO> GruposProdutosDTO { get; set; }
        #endregion



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlteraEstoqueDTO>().HasNoKey();
            modelBuilder.Entity<EntradaNotasSelecaoDTO>().HasNoKey();
            modelBuilder.Entity<RelatorioE01DTO>().HasNoKey();
            modelBuilder.Entity<RelatorioE03DTO>().HasNoKey();
            modelBuilder.Entity<RelatorioV50DTO>().HasNoKey();
            modelBuilder.Entity<RelatorioF04DTO>().HasNoKey();
            modelBuilder.Entity<RelatorioP900DTO>().HasNoKey();
            modelBuilder.Entity<RelatorioF04DTOAgrupado>().HasNoKey();
            modelBuilder.Entity<ComboDTO>().HasNoKey();
            modelBuilder.Entity<ProdutoDTOVinculo>().HasNoKey();

            #region Configurations
            modelBuilder.ApplyConfiguration(new CaixaConfiguration());
            modelBuilder.ApplyConfiguration(new CaixaEventosConfiguration());
            modelBuilder.ApplyConfiguration(new CaixaSituacaoConfiguration());
            modelBuilder.ApplyConfiguration(new CestNcmConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteCategoriasFormpagConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteComputadorConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteComputadorDiscoConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteComputadorSistemaConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteComputadorSistemaModuConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteContabancariaConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteGruposConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteHistoricoConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteUsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new ClientesConfiguration());
            modelBuilder.ApplyConfiguration(new ClientesRefConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigCadastroConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigEmpresaConsultaConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigLocalConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigPedidoConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigSincronizacaoConfiguration());
            modelBuilder.ApplyConfiguration(new ConfigSpedConfiguration());
            modelBuilder.ApplyConfiguration(new ConfimpdocConfiguration());
            modelBuilder.ApplyConfiguration(new DfeSerieNumeracaoConfiguration());
            modelBuilder.ApplyConfiguration(new EmpresasConfiguration());
            modelBuilder.ApplyConfiguration(new EndCertificadoConfigConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaConfigConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaFiscalConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaGradeConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaGradeQuantidadeConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaItemConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaItemFiscalConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaItemImportLacresConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaItemImportacaoConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaItemMedicamentoConfiguration());
            modelBuilder.ApplyConfiguration(new EntradaNumserieConfiguration());
            modelBuilder.ApplyConfiguration(new EstoqueConfiguration());
            modelBuilder.ApplyConfiguration(new EstoqueAlteracaomanualConfiguration());
            modelBuilder.ApplyConfiguration(new EstoqueDataConfiguration());
            modelBuilder.ApplyConfiguration(new FornecedoresConfiguration());
            modelBuilder.ApplyConfiguration(new FotosTransicaoConfiguration());
            modelBuilder.ApplyConfiguration(new GradesConfiguration());
            modelBuilder.ApplyConfiguration(new GridsConfiguration());
            modelBuilder.ApplyConfiguration(new GruposProdutosConfiguration());
            modelBuilder.ApplyConfiguration(new GruposPromocionaisConfiguration());
            modelBuilder.ApplyConfiguration(new NfceCanceladaConfiguration());
            modelBuilder.ApplyConfiguration(new NfceCanceladaXmlConfiguration());
            modelBuilder.ApplyConfiguration(new NfceConfigConfiguration());
            modelBuilder.ApplyConfiguration(new NfceConfigLocalConfiguration());
            modelBuilder.ApplyConfiguration(new NfceContingControllerConfiguration());
            modelBuilder.ApplyConfiguration(new NfceInutilizadaConfiguration());
            modelBuilder.ApplyConfiguration(new NfceInutilizadaXmlConfiguration());
            modelBuilder.ApplyConfiguration(new NfcePedidoConfiguration());
            modelBuilder.ApplyConfiguration(new NfcePedidoItemConfiguration());
            modelBuilder.ApplyConfiguration(new NfceXmlConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoCategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoCompraPagamentoConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoItemNumserieConfiguration());
            modelBuilder.ApplyConfiguration(new PedidoPagamentoConfiguration());
            modelBuilder.ApplyConfiguration(new ProdfornecConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutoSimilarConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosFotoConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosMarcasConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosMarketplaceConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosReajustesConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosRefConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosTributacaoConfiguration());
            modelBuilder.ApplyConfiguration(new ProdutosUrlConfiguration());
            modelBuilder.ApplyConfiguration(new TerminaisConfiguration());
            modelBuilder.ApplyConfiguration(new TerminaisLigadosConfiguration());
            modelBuilder.ApplyConfiguration(new TerminaisRefConfiguration());
            modelBuilder.ApplyConfiguration(new TextosConfiguration());
            modelBuilder.ApplyConfiguration(new TipoprecoConfiguration());
            modelBuilder.ApplyConfiguration(new UnidadeConfiguration());
            modelBuilder.ApplyConfiguration(new UsuariosConfiguration());
            modelBuilder.ApplyConfiguration(new UsuariosCategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new UsuariosRefConfiguration());
            #endregion

            OnModelCreatingPartial(modelBuilder);
        }



    }

}