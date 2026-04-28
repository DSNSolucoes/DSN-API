using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("FORNECEDOR");
            entity.HasIndex(e => e.Id, "PK_FORNECEDOR");
            entity.HasIndex(e => e.LojaId, "IX_FORNECEDOR_LOJA");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.NmFornecedor).HasColumnName("NM_FORNECEDOR").HasMaxLength(200);
            entity.Property(e => e.LojaId).HasColumnName("LOJA_ID").HasColumnType("CHAR(36)").HasMaxLength(36).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Fornecedor> entity);
    }
}
