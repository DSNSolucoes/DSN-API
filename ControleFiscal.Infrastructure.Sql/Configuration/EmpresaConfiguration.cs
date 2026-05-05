
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("EMPRESA");

            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Caminho).HasColumnName("CAMINHO");

            entity.Property(e => e.Porta).HasColumnName("PORTA");

            entity.Property(e => e.Nome).HasColumnName("NOME");

            entity.Property(e => e.Host).HasColumnName("HOST");

            entity.Property(e => e.CNPJ).HasColumnName("CNPJ");
            
            entity.Property(e => e.PercentualST).HasColumnName("PERCENTUAL_ST");
             
             
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Empresa> entity);
    }
}
