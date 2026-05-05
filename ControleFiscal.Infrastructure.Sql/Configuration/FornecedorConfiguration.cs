

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
            entity.HasKey(e => new { e.CdFornecedor , e.Id_Empresa });

            entity.ToTable("FORNECEDOR");

            entity.HasIndex(e => e.CdFornecedor, "CD_FORNECEDOR");
            entity.HasIndex(e => e.Id_Empresa, "ID_EMPRESA");

            entity.Property(e => e.CdFornecedor).HasColumnName("CD_FORNECEDOR");             
            entity.Property(e => e.NmFornecedor).HasColumnName("NM_FORNECEDOR");             
            entity.Property(e => e.Id_Empresa).HasColumnName("ID_EMPRESA");

            // Campos adicionados para o módulo Contas a Pagar
            entity.Property(e => e.NomeFantasia).HasColumnName("NOME_FANTASIA").HasMaxLength(150);
            entity.Property(e => e.Documento).HasColumnName("DOCUMENTO").HasMaxLength(30);
            entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(150);
            entity.Property(e => e.Telefone).HasColumnName("TELEFONE").HasMaxLength(30);
            entity.Property(e => e.Observacoes).HasColumnName("OBSERVACOES");
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20);
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");
             
            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Fornecedor> entity);
    }
}
