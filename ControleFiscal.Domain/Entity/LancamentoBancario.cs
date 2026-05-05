namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class LancamentoBancario
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdContaBancaria { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdArquivoImportado { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public DateTime? DataCompensacao { get; set; }
        public string Tipo { get; set; } = string.Empty; // CREDITO | DEBITO
        public decimal Valor { get; set; }
        public string DescricaoOriginal { get; set; } = string.Empty;
        public string? DescricaoNormalizada { get; set; }
        public string? Documento { get; set; }
        public string Origem { get; set; } = "MANUAL";
        public string Status { get; set; } = "PENDENTE";
        public string? HashImportacao { get; set; }
        public string? Fitid { get; set; }
        public string? Observacoes { get; set; }
        public int? IdResponsavelCriacao { get; set; }
        public int? IdResponsavelAtualizacao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public Empresa? Empresa { get; set; }
        public ContaBancaria? ContaBancaria { get; set; }
        public CategoriaFinanceira? Categoria { get; set; }
        public ArquivoBancarioImportado? ArquivoImportado { get; set; }
    }
}
