using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class PermissaoConfiguration : IEntityTypeConfiguration<Permissao>
    {
        public void Configure(EntityTypeBuilder<Permissao> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("PERMISSAO");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(1000);
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasColumnName("UPDATED_AT").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.SyncStatus).HasColumnName("SYNC_STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");

            entity.HasMany(e => e.PermissoesUsuarios)
                  .WithOne(pu => pu.Permissao)
                  .HasForeignKey(pu => pu.PermissaoId);

            OnConfigurePartial(entity);
        }
        partial void OnConfigurePartial(EntityTypeBuilder<Permissao> entity);
    }
}
