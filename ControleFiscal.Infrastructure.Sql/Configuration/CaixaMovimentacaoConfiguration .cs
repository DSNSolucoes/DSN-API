using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public partial class CaixaMovimentacaoConfiguration : IEntityTypeConfiguration<CaixaMovimentacao>
    {
        public void Configure(EntityTypeBuilder<CaixaMovimentacao> entity)
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("CAIXA_MOVIMENTACAO");

            entity.HasIndex(e => e.Id, "PK_CAIXA_MOVIMENTACAO");
            entity.HasIndex(e => e.CaixaId, "IX_CAIXA_MOVIMENTACAO_CAIXA");
            entity.HasIndex(e => e.TipoValorCaixaId, "IX_CAIXA_MOVIMENTACAO_TIPO");

            entity.Property(e => e.Id)
                .HasColumnName("ID");

            entity.Property(e => e.CaixaId)
                .HasColumnName("CAIXA_ID")
                .IsRequired();

            entity.Property(e => e.TipoValorCaixaId)
                .HasColumnName("TIPO_VALOR_CAIXA_ID")
                .IsRequired();

            entity.Property(e => e.Valor)
                .HasColumnName("VALOR")
                .HasColumnType("NUMERIC(15,2)")
                .IsRequired();

            entity.Property(e => e.DataCadastro)
                .HasColumnName("DATA_CADASTRO")
                .HasColumnType("TIMESTAMP");

            entity.Property(e => e.Ativo)
                .HasColumnName("ATIVO")
                .HasMaxLength(1)
                .HasDefaultValueSql("'V'");

            entity.Property(e => e.DataCompetencia)
                .HasColumnName("DATA_COMPETENCIA")
                .HasColumnType("TIMESTAMP");

            entity.Property(e => e.Descricao)
                .HasColumnName("DESCRICAO")
                .HasMaxLength(100);

            entity.Property(e => e.DataRealizacao)
                .HasColumnName("DATA_REALIZACAO")
                .HasColumnType("TIMESTAMP");

            entity.Property(e => e.AnexoNome)
                .HasColumnName("ANEXO_NOME")
                .HasMaxLength(100);

            entity.Property(e => e.AnexoArquivo)
                .HasColumnName("ANEXO_ARQUIVO")
                .HasColumnType("BLOB SUB_TYPE TEXT");

            entity.Property(e => e.AnexoContentType)
                .HasColumnName("ANEXO_CONTENT_TYPE")
                .HasMaxLength(10);

            entity.Property(e => e.NomeFuncionario)
                .HasColumnName("NOME_FUNCIONARIO")
                .HasMaxLength(100);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CaixaMovimentacao> entity);
    }
}