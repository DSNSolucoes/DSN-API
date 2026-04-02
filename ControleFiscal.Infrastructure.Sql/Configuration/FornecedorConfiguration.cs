

using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> entity)
        {
            entity.HasKey(e => new { e.CdFornecedor , e.idLoja});

            entity.ToTable("FORNECEDOR");

            entity.HasIndex(e => e.CdFornecedor, "CD_FORNECEDOR");
            entity.HasIndex(e => e.idLoja, "ID_LOJA");

            entity.Property(e => e.CdFornecedor).HasColumnName("CD_FORNECEDOR");             
            entity.Property(e => e.NmFornecedor).HasColumnName("NM_FORNECEDOR");             
            entity.Property(e => e.idLoja).HasColumnName("ID_LOJA");             
             
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Fornecedor> entity);
    }
}
