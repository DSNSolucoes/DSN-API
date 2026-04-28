using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class CaixaConfiguration : IEntityTypeConfiguration<Caixa>
    {
        public void Configure(EntityTypeBuilder<Caixa> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CAIXA");
            entity.HasIndex(e => e.Id, "PK_CAIXA");
            entity.HasIndex(e => e.LojaId, "IX_CAIXA_LOJA");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.LojaId).HasColumnName("LOJA_ID").HasColumnType("CHAR(36)").HasMaxLength(36).IsRequired();
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(100).IsRequired();
            entity.Property(e => e.DataCadastro).HasColumnName("DATA_CADASTRO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.Ativo).HasColumnName("ATIVO").HasMaxLength(1).HasDefaultValueSql("'V'");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Caixa> entity);
    }
}
