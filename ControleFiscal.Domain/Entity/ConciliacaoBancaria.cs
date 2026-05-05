namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class ConciliacaoBancaria
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdLancamentoManual { get; set; }
        public int? IdLancamentoImportado { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public int? IdResponsavel { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public Empresa? Empresa { get; set; }
        public LancamentoBancario? LancamentoManual { get; set; }
        public LancamentoBancario? LancamentoImportado { get; set; }
    }
}
