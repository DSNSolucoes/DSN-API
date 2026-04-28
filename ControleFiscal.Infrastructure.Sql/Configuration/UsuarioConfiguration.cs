using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("USUARIOS");
            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(100);
            entity.Property(e => e.Senha).HasColumnName("SENHA").HasMaxLength(1000);
            entity.Property(e => e.Login).HasColumnName("LOGIN").HasMaxLength(20);
            entity.Property(e => e.Produto).HasColumnName("PRODUTO");
            entity.Property(e => e.Relatorio).HasColumnName("RELATORIO");
            entity.Property(e => e.Fiscal).HasColumnName("FISCAL");
            entity.Property(e => e.Financeiro).HasColumnName("FINANCEIRO");
            entity.Property(e => e.Bloqueado).HasColumnName("BLOQUEADO");
            entity.Property(e => e.Dados_Bloqueio).HasColumnName("DADOS_BLOQUEIO");
            entity.Property(e => e.EmpresaId).HasColumnName("EMPRESA_ID");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Usuario> entity);
    }
}
