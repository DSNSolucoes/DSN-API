using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public class CategoriaContaPagarConfiguration : IEntityTypeConfiguration<CategoriaContaPagar>
    {
        public void Configure(EntityTypeBuilder<CategoriaContaPagar> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CATEGORIA_CONTA_PAGAR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(150).IsRequired();
            entity.Property(e => e.IdCategoriaPai).HasColumnName("ID_CATEGORIA_PAI");
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("ATIVA");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            entity.HasOne(e => e.CategoriaPai).WithMany().HasForeignKey(e => e.IdCategoriaPai);

            entity.HasIndex(e => e.IdEmpresa).HasDatabaseName("IDX_CATEGORIA_CP_EMPRESA");
            entity.HasIndex(e => e.Status).HasDatabaseName("IDX_CATEGORIA_CP_STATUS");
        }
    }

    public class CentroCustoCPConfiguration : IEntityTypeConfiguration<CentroCustoCP>
    {
        public void Configure(EntityTypeBuilder<CentroCustoCP> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CENTRO_CUSTO_CP");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Codigo).HasColumnName("CODIGO").HasMaxLength(30);
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("ATIVO");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);

            entity.HasIndex(e => e.IdEmpresa).HasDatabaseName("IDX_CC_CP_EMPRESA");
            entity.HasIndex(e => e.Status).HasDatabaseName("IDX_CC_CP_STATUS");
        }
    }

    public class ContaPagarConfiguration : IEntityTypeConfiguration<ContaPagar>
    {
        public void Configure(EntityTypeBuilder<ContaPagar> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CONTA_PAGAR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdFornecedor).HasColumnName("ID_FORNECEDOR");
            entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.IdCentroCusto).HasColumnName("ID_CENTRO_CUSTO");
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(300).IsRequired();
            entity.Property(e => e.NumeroDocumento).HasColumnName("NUMERO_DOCUMENTO").HasMaxLength(100);
            entity.Property(e => e.SerieDocumento).HasColumnName("SERIE_DOCUMENTO").HasMaxLength(30);
            entity.Property(e => e.TipoDocumento).HasColumnName("TIPO_DOCUMENTO").HasMaxLength(40);
            entity.Property(e => e.DataEmissao).HasColumnName("DATA_EMISSAO").HasColumnType("DATE");
            entity.Property(e => e.DataCompetencia).HasColumnName("DATA_COMPETENCIA").HasColumnType("DATE");
            entity.Property(e => e.DataVencimento).HasColumnName("DATA_VENCIMENTO").HasColumnType("DATE").IsRequired();
            entity.Property(e => e.ValorOriginal).HasColumnName("VALOR_ORIGINAL").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.ValorDesconto).HasColumnName("VALOR_DESCONTO").HasColumnType("NUMERIC(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.ValorAcrescimo).HasColumnName("VALOR_ACRESCIMO").HasColumnType("NUMERIC(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.ValorMulta).HasColumnName("VALOR_MULTA").HasColumnType("NUMERIC(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.ValorJuros).HasColumnName("VALOR_JUROS").HasColumnType("NUMERIC(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.ValorTotal).HasColumnName("VALOR_TOTAL").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.ValorPago).HasColumnName("VALOR_PAGO").HasColumnType("NUMERIC(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.SaldoAPagar).HasColumnName("SALDO_A_PAGAR").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.NumeroParcela).HasColumnName("NUMERO_PARCELA").HasDefaultValue(1);
            entity.Property(e => e.TotalParcelas).HasColumnName("TOTAL_PARCELAS").HasDefaultValue(1);
            entity.Property(e => e.IdContaOrigem).HasColumnName("ID_CONTA_ORIGEM");
            entity.Property(e => e.Recorrente).HasColumnName("RECORRENTE").HasDefaultValue((short)0);
            entity.Property(e => e.IdRecorrencia).HasColumnName("ID_RECORRENCIA");
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(30).HasDefaultValue("ABERTA");
            entity.Property(e => e.Observacoes).HasColumnName("OBSERVACOES");
            entity.Property(e => e.MotivoCancelamento).HasColumnName("MOTIVO_CANCELAMENTO");
            entity.Property(e => e.IdResponsavelCriacao).HasColumnName("ID_RESPONSAVEL_CRIACAO");
            entity.Property(e => e.IdResponsavelAtualizacao).HasColumnName("ID_RESPONSAVEL_ATUALIZACAO");
            entity.Property(e => e.IdResponsavelCancelamento).HasColumnName("ID_RESPONSAVEL_CANCELAMENTO");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataCancelamento).HasColumnName("DATA_CANCELAMENTO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            // IdFornecedor armazena CD_FORNECEDOR da tabela FORNECEDOR (sem FK por PK composta)
            entity.HasOne(e => e.Categoria).WithMany(c => c.ContasPagar).HasForeignKey(e => e.IdCategoria).IsRequired(false);
            entity.HasOne(e => e.CentroCusto).WithMany(c => c.ContasPagar).HasForeignKey(e => e.IdCentroCusto).IsRequired(false);
            entity.HasMany(e => e.Pagamentos).WithOne(p => p.ContaPagar).HasForeignKey(p => p.IdContaPagar);

            entity.HasIndex(e => e.IdEmpresa).HasDatabaseName("IDX_CP_EMPRESA");
            entity.HasIndex(e => new { e.IdEmpresa, e.DataVencimento }).HasDatabaseName("IDX_CP_EMPRESA_VENC");
            entity.HasIndex(e => e.Status).HasDatabaseName("IDX_CP_STATUS");
        }
    }

    public class PagamentoContaPagarConfiguration : IEntityTypeConfiguration<PagamentoContaPagar>
    {
        public void Configure(EntityTypeBuilder<PagamentoContaPagar> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("PAGAMENTO_CONTA_PAGAR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdContaPagar).HasColumnName("ID_CONTA_PAGAR").IsRequired();
            entity.Property(e => e.DataPagamento).HasColumnName("DATA_PAGAMENTO").HasColumnType("DATE").IsRequired();
            entity.Property(e => e.ValorPago).HasColumnName("VALOR_PAGO").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.ValorDesconto).HasColumnName("VALOR_DESCONTO").HasColumnType("NUMERIC(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.ValorJuros).HasColumnName("VALOR_JUROS").HasColumnType("NUMERIC(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.ValorMulta).HasColumnName("VALOR_MULTA").HasColumnType("NUMERIC(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.FormaPagamento).HasColumnName("FORMA_PAGAMENTO").HasMaxLength(40);
            entity.Property(e => e.DocumentoPagamento).HasColumnName("DOCUMENTO_PAGAMENTO").HasMaxLength(100);
            entity.Property(e => e.Comprovante).HasColumnName("COMPROVANTE").HasMaxLength(255);
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("ATIVO");
            entity.Property(e => e.Observacoes).HasColumnName("OBSERVACOES");
            entity.Property(e => e.IdResponsavel).HasColumnName("ID_RESPONSAVEL");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IdResponsavelEstorno).HasColumnName("ID_RESPONSAVEL_ESTORNO");
            entity.Property(e => e.DataEstorno).HasColumnName("DATA_ESTORNO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.MotivoEstorno).HasColumnName("MOTIVO_ESTORNO");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            entity.HasOne(e => e.ContaPagar).WithMany(c => c.Pagamentos).HasForeignKey(e => e.IdContaPagar);

            entity.HasIndex(e => e.IdEmpresa).HasDatabaseName("IDX_PAG_CP_EMPRESA");
            entity.HasIndex(e => e.IdContaPagar).HasDatabaseName("IDX_PAG_CP_CONTA");
            entity.HasIndex(e => e.Status).HasDatabaseName("IDX_PAG_CP_STATUS");
        }
    }

    public class ContaPagarRecorrenteConfiguration : IEntityTypeConfiguration<ContaPagarRecorrente>
    {
        public void Configure(EntityTypeBuilder<ContaPagarRecorrente> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CONTA_PAGAR_RECORRENTE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdFornecedor).HasColumnName("ID_FORNECEDOR");
            entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.IdCentroCusto).HasColumnName("ID_CENTRO_CUSTO");
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO").HasMaxLength(300).IsRequired();
            entity.Property(e => e.Valor).HasColumnName("VALOR").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.Periodicidade).HasColumnName("PERIODICIDADE").HasMaxLength(30).IsRequired();
            entity.Property(e => e.DiaVencimento).HasColumnName("DIA_VENCIMENTO");
            entity.Property(e => e.DataInicio).HasColumnName("DATA_INICIO").HasColumnType("DATE").IsRequired();
            entity.Property(e => e.DataFim).HasColumnName("DATA_FIM").HasColumnType("DATE");
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("ATIVA");
            entity.Property(e => e.Observacoes).HasColumnName("OBSERVACOES");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            // IdFornecedor armazena CD_FORNECEDOR da tabela FORNECEDOR (sem FK por PK composta)
            entity.HasOne(e => e.Categoria).WithMany().HasForeignKey(e => e.IdCategoria).IsRequired(false);
            entity.HasOne(e => e.CentroCusto).WithMany().HasForeignKey(e => e.IdCentroCusto).IsRequired(false);
        }
    }

    public class AuditoriaContasPagarConfiguration : IEntityTypeConfiguration<AuditoriaContasPagar>
    {
        public void Configure(EntityTypeBuilder<AuditoriaContasPagar> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("AUDITORIA_CONTAS_PAGAR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdResponsavel).HasColumnName("ID_RESPONSAVEL");
            entity.Property(e => e.Entidade).HasColumnName("ENTIDADE").HasMaxLength(100).IsRequired();
            entity.Property(e => e.IdEntidade).HasColumnName("ID_ENTIDADE");
            entity.Property(e => e.Acao).HasColumnName("ACAO").HasMaxLength(100).IsRequired();
            entity.Property(e => e.DadosAnteriores).HasColumnName("DADOS_ANTERIORES");
            entity.Property(e => e.DadosNovos).HasColumnName("DADOS_NOVOS");
            entity.Property(e => e.Ip).HasColumnName("IP").HasMaxLength(50);
            entity.Property(e => e.UserAgent).HasColumnName("USER_AGENT").HasMaxLength(800);
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
        }
    }
}
