
using ControleFiscal.Infrastructure.Sql.Entity;
using ControleFiscal.Infrastructure.Sql.Local;
using Microsoft.EntityFrameworkCore;
#nullable disable

namespace ControleFiscal.Infrastructure.Sql
{
    public partial class ContextLocalContext : DbContext
    { 
        public ContextLocalContext(
            DbContextOptions<ContextLocalContext> options  )
            : base(options)
        { 
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string conn = "User=SYSDBA;Password=masterkey;Database=local;DataSource=127.0.0.1;Port=3050;Dialect=3;Charset=UTF8;";
            optionsBuilder.UseFirebird(conn, o => o.WithExplicitStringLiteralTypes()).AddInterceptors(new SqlInterceptor());
            optionsBuilder.LogTo(Console.Write);


        }


        public virtual DbSet<Empresa> Empresas { get; set; }
        public virtual DbSet<Fornecedor> Fornecedores { get; set; }
        public virtual DbSet<Ncm> NCMs { get; set; }
        public virtual DbSet<Usuario> Logins { get; set; }
        public virtual DbSet<Permissao> Permissoes { get; set; }
        public virtual DbSet<PermissaoUsuario> PermissoesUsuarios { get; set; }
        public virtual DbSet<Caixa> Caixa { get; set; }
        public virtual DbSet<CaixaMovimentacao> CaixaMovimentacao { get; set; }

        public virtual DbSet<TipoValorCaixa> TipoValorCaixa { get; set; }
        public virtual DbSet<TipoValorCaixaItem> TipoValorCaixaItem { get; set; }

        // ── Controle Bancário ────────────────────────────────────────────────
        public virtual DbSet<Banco> Bancos { get; set; }
        public virtual DbSet<ContaBancaria> ContasBancarias { get; set; }
        public virtual DbSet<CategoriaFinanceira> CategoriasFinanceiras { get; set; }
        public virtual DbSet<LancamentoBancario> LancamentosBancarios { get; set; }
        public virtual DbSet<ArquivoBancarioImportado> ArquivosBancariosImportados { get; set; }
        public virtual DbSet<ItemImportacaoBancaria> ItensImportacaoBancaria { get; set; }
        public virtual DbSet<ConciliacaoBancaria> ConciliacoesBancarias { get; set; }
        public virtual DbSet<FechamentoBancarioMensal> FechamentosBancarios { get; set; }
        public virtual DbSet<RegraClassificacaoBancaria> RegrasClassificacaoBancaria { get; set; }
        public virtual DbSet<AuditoriaFinanceira> AuditoriasFinanceiras { get; set; }

        // ── Contas a Pagar ───────────────────────────────────────────────────
        public virtual DbSet<CategoriaContaPagar> CategoriasContaPagar { get; set; }
        public virtual DbSet<CentroCustoCP> CentrosCustoCP { get; set; }
        public virtual DbSet<ContaPagar> ContasPagar { get; set; }
        public virtual DbSet<PagamentoContaPagar> PagamentosContaPagar { get; set; }
        public virtual DbSet<ContaPagarRecorrente> ContasPagarRecorrentes { get; set; }
        public virtual DbSet<AuditoriaContasPagar> AuditoriaContasPagar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmpresaConfiguration());
            modelBuilder.ApplyConfiguration(new FornecedorConfiguration());
            modelBuilder.ApplyConfiguration(new NcmConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new PermissaoConfiguration());
            modelBuilder.ApplyConfiguration(new PermissaoUsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new CaixaConfiguration());
            modelBuilder.ApplyConfiguration(new CaixaMovimentacaoConfiguration());

            modelBuilder.ApplyConfiguration(new TipoValorCaixaConfiguration());
            modelBuilder.ApplyConfiguration(new TipoValorCaixaItemConfiguration());

            // ── Controle Bancário ────────────────────────────────────────────
            modelBuilder.ApplyConfiguration(new BancoConfiguration());
            modelBuilder.ApplyConfiguration(new ContaBancariaConfiguration());
            modelBuilder.ApplyConfiguration(new CategoriaFinanceiraConfiguration());
            modelBuilder.ApplyConfiguration(new LancamentoBancarioConfiguration());
            modelBuilder.ApplyConfiguration(new ArquivoBancarioImportadoConfiguration());
            modelBuilder.ApplyConfiguration(new ItemImportacaoBancariaConfiguration());
            modelBuilder.ApplyConfiguration(new ConciliacaoBancariaConfiguration());
            modelBuilder.ApplyConfiguration(new FechamentoBancarioMensalConfiguration());
            modelBuilder.ApplyConfiguration(new RegraClassificacaoBancariaConfiguration());
            modelBuilder.ApplyConfiguration(new AuditoriaFinanceiraConfiguration());

            // ── Contas a Pagar ───────────────────────────────────────────────
            modelBuilder.ApplyConfiguration(new CategoriaContaPagarConfiguration());
            modelBuilder.ApplyConfiguration(new CentroCustoCPConfiguration());
            modelBuilder.ApplyConfiguration(new ContaPagarConfiguration());
            modelBuilder.ApplyConfiguration(new PagamentoContaPagarConfiguration());
            modelBuilder.ApplyConfiguration(new ContaPagarRecorrenteConfiguration());
            modelBuilder.ApplyConfiguration(new AuditoriaContasPagarConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}