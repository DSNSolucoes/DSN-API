using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class TipoValorCaixaConfiguration : IEntityTypeConfiguration<TipoValorCaixa>
    {
        public void Configure(EntityTypeBuilder<TipoValorCaixa> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("TIPO_VALOR_CAIXA");

            entity.HasIndex(e => e.Id, "PK_TIPO_VALOR_CAIXA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<TipoValorCaixa> entity);
    }
}