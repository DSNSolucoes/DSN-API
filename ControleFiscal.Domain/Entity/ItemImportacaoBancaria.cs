namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class ItemImportacaoBancaria
    {
        public int Id { get; set; }
        public int IdArquivoImportado { get; set; }
        public int? NumeroLinha { get; set; }
        public string? ConteudoOriginal { get; set; }
        public DateTime? DataMovimentacao { get; set; }
        public string? Descricao { get; set; }
        public string? Documento { get; set; }
        public string? Tipo { get; set; }
        public decimal? Valor { get; set; }
        public string? HashCalculado { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? MensagemErro { get; set; }
        public int? IdLancamentoBancario { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public ArquivoBancarioImportado? ArquivoImportado { get; set; }
        public LancamentoBancario? LancamentoBancario { get; set; }
    }
}
