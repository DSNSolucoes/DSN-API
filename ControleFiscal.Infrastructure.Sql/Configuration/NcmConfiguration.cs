
using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class NcmConfiguration : IEntityTypeConfiguration<Ncm>
    {
        public void Configure(EntityTypeBuilder<Ncm> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("NCM");

            entity.HasIndex(e => e.Id, "ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NCM).HasColumnName("NCM");              
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO");              
            entity.Property(e => e.Padrao).HasColumnName("PADRAO");              
             
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Ncm> entity);
    }
}
