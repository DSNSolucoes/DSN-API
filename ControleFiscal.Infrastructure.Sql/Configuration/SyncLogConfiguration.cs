using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public class SyncLogConfiguration : IEntityTypeConfiguration<SyncLog>
    {
        public void Configure(EntityTypeBuilder<SyncLog> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("SYNC_LOG");
            entity.HasIndex(e => e.Status, "IX_SYNC_LOG_STATUS");
            entity.HasIndex(e => e.Tabela, "IX_SYNC_LOG_TABELA");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.Tabela).HasColumnName("TABELA").HasMaxLength(50).IsRequired();
            entity.Property(e => e.RegistroId).HasColumnName("REGISTRO_ID").HasColumnType("CHAR(36)").HasMaxLength(36).IsRequired();
            entity.Property(e => e.Operacao).HasColumnName("OPERACAO").HasMaxLength(10).IsRequired();
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(10).HasDefaultValueSql("'PENDING'");
            entity.Property(e => e.Payload).HasColumnName("PAYLOAD").HasColumnType("BLOB SUB_TYPE TEXT");
            entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT").HasColumnType("TIMESTAMP").HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.SyncedAt).HasColumnName("SYNCED_AT").HasColumnType("TIMESTAMP");
        }
    }
}
