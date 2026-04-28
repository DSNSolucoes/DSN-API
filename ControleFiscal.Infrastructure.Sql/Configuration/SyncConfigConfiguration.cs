using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public class SyncConfigConfiguration : IEntityTypeConfiguration<SyncConfig>
    {
        public void Configure(EntityTypeBuilder<SyncConfig> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("SYNC_CONFIG");

            entity.Property(e => e.Id).HasColumnName("ID").HasColumnType("CHAR(36)").HasMaxLength(36).ValueGeneratedNever();
            entity.Property(e => e.UltimoSync).HasColumnName("ULTIMO_SYNC").HasColumnType("TIMESTAMP");
            entity.Property(e => e.EmSincronizacao).HasColumnName("EM_SINCRONIZACAO").HasColumnType("SMALLINT").HasDefaultValueSql("0");
            entity.Property(e => e.UrlNuvem).HasColumnName("URL_NUVEM").HasMaxLength(500);
        }
    }
}
