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

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.IdEmpresa)
                .HasColumnName("ID_EMPRESA")
                .IsRequired();

            entity.Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.DataCadastro)
                .HasColumnName("DATA_CADASTRO")
                .HasColumnType("TIMESTAMP");

            entity.Property(e => e.Ordem)
                .HasColumnName("ORDEM");

            entity.Property(e => e.Ativo)
                .HasColumnName("ATIVO")
                .HasMaxLength(1)
                .HasDefaultValueSql("'V'");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Caixa> entity);
    }
}