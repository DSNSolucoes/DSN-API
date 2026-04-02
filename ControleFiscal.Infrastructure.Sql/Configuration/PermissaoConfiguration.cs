
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class PermissaoConfiguration : IEntityTypeConfiguration<Permissao>
    {
        public void Configure(EntityTypeBuilder<Permissao> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("PERMISSAO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(1000);
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(1000);

            entity.HasMany(e => e.PermissoesUsuarios)
                  .WithOne(pu => pu.Permissao)
                  .HasForeignKey(pu => pu.PermissaoId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Permissao> entity);
    }
}
