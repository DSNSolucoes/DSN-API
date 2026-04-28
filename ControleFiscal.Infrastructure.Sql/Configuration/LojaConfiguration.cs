using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class LojaConfiguration : IEntityTypeConfiguration<Lojas>
    {
        public void Configure(EntityTypeBuilder<Lojas> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("LOJA");
            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.Caminho).HasColumnName("CAMINHO");
            entity.Property(e => e.Porta).HasColumnName("PORTA");
            entity.Property(e => e.Nome).HasColumnName("NOME");
            entity.Property(e => e.Host).HasColumnName("HOST");
            entity.Property(e => e.CNPJ).HasColumnName("CNPJ");
            entity.Property(e => e.PercentualST).HasColumnName("PERCENTUAL_ST");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Lojas> entity);
    }
}
