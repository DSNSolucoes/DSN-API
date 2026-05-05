using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class TipoValorCaixaItemConfiguration : IEntityTypeConfiguration<TipoValorCaixaItem>
    {
        public void Configure(EntityTypeBuilder<TipoValorCaixaItem> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("TIPO_VALOR_CAIXA_ITEM");

            entity.HasIndex(e => e.Id, "PK_TIPO_VALOR_CAIXA_ITEM");
            entity.HasIndex(e => e.TipoValorCaixaId, "IDX_TVCI_TIPO_VALOR_CAIXA");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.TipoValorCaixaId)
                .HasColumnName("TIPO_VALOR_CAIXA_ID")
                .IsRequired();

            entity.Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Ativo)
                .HasColumnName("ATIVO")
                .HasMaxLength(1)
                .HasDefaultValueSql("'S'");

            entity.Property(e => e.DataCadastro)
                .HasColumnName("DATA_CADASTRO")
                .HasColumnType("TIMESTAMP"); 

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TipoValorCaixaItem> entity);
    }
}
