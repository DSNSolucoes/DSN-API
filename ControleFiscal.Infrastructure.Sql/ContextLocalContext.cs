
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


        public virtual DbSet<Lojas> Lojas { get; set; }
        public virtual DbSet<Fornecedor> Fornecedores { get; set; }
        public virtual DbSet<Ncm> NCMs { get; set; }
        public virtual DbSet<Usuario> Logins { get; set; }
        public virtual DbSet<Permissao> Permissoes { get; set; }
        public virtual DbSet<PermissaoUsuario> PermissoesUsuarios { get; set; }
        public virtual DbSet<Caixa> Caixa { get; set; }
        public virtual DbSet<CaixaMovimentacao> CaixaMovimentacao { get; set; }

        public virtual DbSet<TipoValorCaixa> TipoValorCaixa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LojaConfiguration());
            modelBuilder.ApplyConfiguration(new FornecedorConfiguration());
            modelBuilder.ApplyConfiguration(new NcmConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new PermissaoConfiguration());
            modelBuilder.ApplyConfiguration(new PermissaoUsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new CaixaConfiguration());
            modelBuilder.ApplyConfiguration(new CaixaMovimentacaoConfiguration());

            modelBuilder.ApplyConfiguration(new TipoValorCaixaConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}