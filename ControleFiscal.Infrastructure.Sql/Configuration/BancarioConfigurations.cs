using ControleFiscal.Infrastructure.Sql.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFiscal.Infrastructure.Sql.Local
{
    public class BancoConfiguration : IEntityTypeConfiguration<Banco>
    {
        public void Configure(EntityTypeBuilder<Banco> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("BANCO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Codigo).HasColumnName("CODIGO").HasMaxLength(10);
            entity.Property(e => e.Ispb).HasColumnName("ISPB").HasMaxLength(20);
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(200).IsRequired();
            entity.Property(e => e.NomeReduzido).HasColumnName("NOME_REDUZIDO").HasMaxLength(100);
            entity.Property(e => e.ParticipaCompe).HasColumnName("PARTICIPA_COMPE").HasDefaultValue((short)0);
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("ATIVO");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");

            entity.HasIndex(e => e.Status).HasDatabaseName("IDX_BANCO_STATUS");
            entity.HasIndex(e => e.Nome).HasDatabaseName("IDX_BANCO_NOME");
        }
    }

    public class ContaBancariaConfiguration : IEntityTypeConfiguration<ContaBancaria>
    {
        public void Configure(EntityTypeBuilder<ContaBancaria> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CONTA_BANCARIA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdBanco).HasColumnName("ID_BANCO");
            entity.Property(e => e.Agencia).HasColumnName("AGENCIA").HasMaxLength(20);
            entity.Property(e => e.DigitoAgencia).HasColumnName("DIGITO_AGENCIA").HasMaxLength(5);
            entity.Property(e => e.NumeroConta).HasColumnName("NUMERO_CONTA").HasMaxLength(30);
            entity.Property(e => e.DigitoConta).HasColumnName("DIGITO_CONTA").HasMaxLength(5);
            entity.Property(e => e.TipoConta).HasColumnName("TIPO_CONTA").HasMaxLength(30).IsRequired();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(120).IsRequired();
            entity.Property(e => e.Moeda).HasColumnName("MOEDA").HasMaxLength(3).HasDefaultValue("BRL");
            entity.Property(e => e.SaldoInicial).HasColumnName("SALDO_INICIAL").HasColumnType("NUMERIC(18,2)").HasDefaultValue(0m);
            entity.Property(e => e.DataSaldoInicial).HasColumnName("DATA_SALDO_INICIAL").HasColumnType("DATE").IsRequired();
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("ATIVA");
            entity.Property(e => e.Observacoes).HasColumnName("OBSERVACOES");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            entity.HasOne(e => e.Banco).WithMany(b => b.ContasBancarias).HasForeignKey(e => e.IdBanco);

            entity.HasIndex(e => e.IdEmpresa).HasDatabaseName("IDX_CONTA_EMPRESA");
            entity.HasIndex(e => e.Status).HasDatabaseName("IDX_CONTA_STATUS");
        }
    }

    public class CategoriaFinanceiraConfiguration : IEntityTypeConfiguration<CategoriaFinanceira>
    {
        public void Configure(EntityTypeBuilder<CategoriaFinanceira> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CATEGORIA_FINANCEIRA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.Nome).HasColumnName("NOME").HasMaxLength(150).IsRequired();
            entity.Property(e => e.Tipo).HasColumnName("TIPO").HasMaxLength(20).IsRequired();
            entity.Property(e => e.IdCategoriaPai).HasColumnName("ID_CATEGORIA_PAI");
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("ATIVA");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            entity.HasOne(e => e.CategoriaPai).WithMany(c => c.SubCategorias).HasForeignKey(e => e.IdCategoriaPai);

            entity.HasIndex(e => e.IdEmpresa).HasDatabaseName("IDX_CAT_EMPRESA");
        }
    }

    public class LancamentoBancarioConfiguration : IEntityTypeConfiguration<LancamentoBancario>
    {
        public void Configure(EntityTypeBuilder<LancamentoBancario> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("LANCAMENTO_BANCARIO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdContaBancaria).HasColumnName("ID_CONTA_BANCARIA").IsRequired();
            entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA");
            entity.Property(e => e.IdArquivoImportado).HasColumnName("ID_ARQUIVO_IMPORTADO");
            entity.Property(e => e.DataMovimentacao).HasColumnName("DATA_MOVIMENTACAO").HasColumnType("DATE").IsRequired();
            entity.Property(e => e.DataCompensacao).HasColumnName("DATA_COMPENSACAO").HasColumnType("DATE");
            entity.Property(e => e.Tipo).HasColumnName("TIPO").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Valor).HasColumnName("VALOR").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.DescricaoOriginal).HasColumnName("DESCRICAO_ORIGINAL").IsRequired();
            entity.Property(e => e.DescricaoNormalizada).HasColumnName("DESCRICAO_NORMALIZADA").HasMaxLength(800);
            entity.Property(e => e.Documento).HasColumnName("DOCUMENTO").HasMaxLength(100);
            entity.Property(e => e.Origem).HasColumnName("ORIGEM").HasMaxLength(30).HasDefaultValue("MANUAL");
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(30).HasDefaultValue("PENDENTE");
            entity.Property(e => e.HashImportacao).HasColumnName("HASH_IMPORTACAO").HasMaxLength(128);
            entity.Property(e => e.Fitid).HasColumnName("FITID").HasMaxLength(150);
            entity.Property(e => e.Observacoes).HasColumnName("OBSERVACOES");
            entity.Property(e => e.IdResponsavelCriacao).HasColumnName("ID_RESPONSAVEL_CRIACAO");
            entity.Property(e => e.IdResponsavelAtualizacao).HasColumnName("ID_RESPONSAVEL_ATUALIZACAO");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            entity.HasOne(e => e.ContaBancaria).WithMany(c => c.Lancamentos).HasForeignKey(e => e.IdContaBancaria);
            entity.HasOne(e => e.Categoria).WithMany().HasForeignKey(e => e.IdCategoria);
            entity.HasOne(e => e.ArquivoImportado).WithMany().HasForeignKey(e => e.IdArquivoImportado);

            entity.HasIndex(e => new { e.IdEmpresa, e.DataMovimentacao }).HasDatabaseName("IDX_LANC_EMPRESA_DATA");
            entity.HasIndex(e => new { e.IdContaBancaria, e.DataMovimentacao }).HasDatabaseName("IDX_LANC_CONTA_DATA");
            entity.HasIndex(e => e.Status).HasDatabaseName("IDX_LANC_STATUS");
        }
    }

    public class ArquivoBancarioImportadoConfiguration : IEntityTypeConfiguration<ArquivoBancarioImportado>
    {
        public void Configure(EntityTypeBuilder<ArquivoBancarioImportado> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("ARQUIVO_BANCARIO_IMPORTADO");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdContaBancaria).HasColumnName("ID_CONTA_BANCARIA").IsRequired();
            entity.Property(e => e.NomeOriginal).HasColumnName("NOME_ORIGINAL").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Formato).HasColumnName("FORMATO").HasMaxLength(20).IsRequired();
            entity.Property(e => e.TamanhoBytes).HasColumnName("TAMANHO_BYTES");
            entity.Property(e => e.HashArquivo).HasColumnName("HASH_ARQUIVO").HasMaxLength(128);
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(40).HasDefaultValue("RECEBIDO");
            entity.Property(e => e.TotalLidos).HasColumnName("TOTAL_LIDOS").HasDefaultValue(0);
            entity.Property(e => e.TotalImportados).HasColumnName("TOTAL_IMPORTADOS").HasDefaultValue(0);
            entity.Property(e => e.TotalDuplicados).HasColumnName("TOTAL_DUPLICADOS").HasDefaultValue(0);
            entity.Property(e => e.TotalErros).HasColumnName("TOTAL_ERROS").HasDefaultValue(0);
            entity.Property(e => e.LogErro).HasColumnName("LOG_ERRO");
            entity.Property(e => e.IdResponsavel).HasColumnName("ID_RESPONSAVEL");
            entity.Property(e => e.DataUpload).HasColumnName("DATA_UPLOAD").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataProcessamento).HasColumnName("DATA_PROCESSAMENTO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            entity.HasOne(e => e.ContaBancaria).WithMany().HasForeignKey(e => e.IdContaBancaria);
        }
    }

    public class ItemImportacaoBancariaConfiguration : IEntityTypeConfiguration<ItemImportacaoBancaria>
    {
        public void Configure(EntityTypeBuilder<ItemImportacaoBancaria> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("ITEM_IMPORTACAO_BANCARIA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdArquivoImportado).HasColumnName("ID_ARQUIVO_IMPORTADO").IsRequired();
            entity.Property(e => e.NumeroLinha).HasColumnName("NUMERO_LINHA");
            entity.Property(e => e.ConteudoOriginal).HasColumnName("CONTEUDO_ORIGINAL");
            entity.Property(e => e.DataMovimentacao).HasColumnName("DATA_MOVIMENTACAO").HasColumnType("DATE");
            entity.Property(e => e.Descricao).HasColumnName("DESCRICAO");
            entity.Property(e => e.Documento).HasColumnName("DOCUMENTO").HasMaxLength(100);
            entity.Property(e => e.Tipo).HasColumnName("TIPO").HasMaxLength(20);
            entity.Property(e => e.Valor).HasColumnName("VALOR").HasColumnType("NUMERIC(18,2)");
            entity.Property(e => e.HashCalculado).HasColumnName("HASH_CALCULADO").HasMaxLength(128);
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(30).IsRequired();
            entity.Property(e => e.MensagemErro).HasColumnName("MENSAGEM_ERRO");
            entity.Property(e => e.IdLancamentoBancario).HasColumnName("ID_LANCAMENTO_BANCARIO");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.ArquivoImportado).WithMany(a => a.Itens).HasForeignKey(e => e.IdArquivoImportado);
            entity.HasOne(e => e.LancamentoBancario).WithMany().HasForeignKey(e => e.IdLancamentoBancario);
        }
    }

    public class ConciliacaoBancariaConfiguration : IEntityTypeConfiguration<ConciliacaoBancaria>
    {
        public void Configure(EntityTypeBuilder<ConciliacaoBancaria> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CONCILIACAO_BANCARIA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdLancamentoManual).HasColumnName("ID_LANCAMENTO_MANUAL");
            entity.Property(e => e.IdLancamentoImportado).HasColumnName("ID_LANCAMENTO_IMPORTADO");
            entity.Property(e => e.Tipo).HasColumnName("TIPO").HasMaxLength(40).IsRequired();
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(40).IsRequired();
            entity.Property(e => e.Observacao).HasColumnName("OBSERVACAO");
            entity.Property(e => e.IdResponsavel).HasColumnName("ID_RESPONSAVEL");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            entity.HasOne(e => e.LancamentoManual).WithMany().HasForeignKey(e => e.IdLancamentoManual);
            entity.HasOne(e => e.LancamentoImportado).WithMany().HasForeignKey(e => e.IdLancamentoImportado);
        }
    }

    public class FechamentoBancarioMensalConfiguration : IEntityTypeConfiguration<FechamentoBancarioMensal>
    {
        public void Configure(EntityTypeBuilder<FechamentoBancarioMensal> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("FECHAMENTO_BANCARIO_MENSAL");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdContaBancaria).HasColumnName("ID_CONTA_BANCARIA").IsRequired();
            entity.Property(e => e.Ano).HasColumnName("ANO").IsRequired();
            entity.Property(e => e.Mes).HasColumnName("MES").IsRequired();
            entity.Property(e => e.SaldoInicial).HasColumnName("SALDO_INICIAL").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.TotalCreditos).HasColumnName("TOTAL_CREDITOS").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.TotalDebitos).HasColumnName("TOTAL_DEBITOS").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.SaldoFinal).HasColumnName("SALDO_FINAL").HasColumnType("NUMERIC(18,2)").IsRequired();
            entity.Property(e => e.QtdLancamentos).HasColumnName("QTD_LANCAMENTOS").HasDefaultValue(0);
            entity.Property(e => e.QtdConciliados).HasColumnName("QTD_CONCILIADOS").HasDefaultValue(0);
            entity.Property(e => e.QtdPendentes).HasColumnName("QTD_PENDENTES").HasDefaultValue(0);
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(30).HasDefaultValue("ABERTO");
            entity.Property(e => e.IdFechadoPor).HasColumnName("ID_FECHADO_POR");
            entity.Property(e => e.DataFechamento).HasColumnName("DATA_FECHAMENTO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.IdReabertoPor).HasColumnName("ID_REABERTO_POR");
            entity.Property(e => e.DataReabertura).HasColumnName("DATA_REABERTURA").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            entity.HasOne(e => e.ContaBancaria).WithMany().HasForeignKey(e => e.IdContaBancaria);

            entity.HasIndex(e => new { e.IdContaBancaria, e.Ano, e.Mes }).IsUnique().HasDatabaseName("UK_FECH_CONTA_MES");
        }
    }

    public class RegraClassificacaoBancariaConfiguration : IEntityTypeConfiguration<RegraClassificacaoBancaria>
    {
        public void Configure(EntityTypeBuilder<RegraClassificacaoBancaria> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("REGRA_CLASSIFICACAO_BANCARIA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdEmpresa).HasColumnName("ID_EMPRESA").IsRequired();
            entity.Property(e => e.IdCategoria).HasColumnName("ID_CATEGORIA").IsRequired();
            entity.Property(e => e.PalavraChave).HasColumnName("PALAVRA_CHAVE").HasMaxLength(200).IsRequired();
            entity.Property(e => e.TipoLancamento).HasColumnName("TIPO_LANCAMENTO").HasMaxLength(20);
            entity.Property(e => e.Prioridade).HasColumnName("PRIORIDADE").HasDefaultValue(0);
            entity.Property(e => e.Status).HasColumnName("STATUS").HasMaxLength(20).HasDefaultValue("ATIVA");
            entity.Property(e => e.DataCriacao).HasColumnName("DATA_CRIACAO").HasColumnType("TIMESTAMP");
            entity.Property(e => e.DataAtualizacao).HasColumnName("DATA_ATUALIZACAO").HasColumnType("TIMESTAMP");

            entity.HasOne(e => e.Empresa).WithMany().HasForeignKey(e => e.IdEmpresa);
            entity.HasOne(e => e.Categoria).WithMany().HasForeignKey(e => e.IdCategoria);
        }
    }

    public class AuditoriaFinanceiraConfiguration : IEntityTypeConfiguration<AuditoriaFinanceira>
    {
        public void Configure(EntityTypeBuilder<AuditoriaFinanceira> entity)
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("AUDITORIA_FINANCEIRA");

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
