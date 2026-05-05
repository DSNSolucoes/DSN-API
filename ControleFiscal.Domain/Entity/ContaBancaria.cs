namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class ContaBancaria
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdBanco { get; set; }
        public string? Agencia { get; set; }
        public string? DigitoAgencia { get; set; }
        public string? NumeroConta { get; set; }
        public string? DigitoConta { get; set; }
        public string TipoConta { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Moeda { get; set; } = "BRL";
        public decimal SaldoInicial { get; set; } = 0;
        public DateTime DataSaldoInicial { get; set; }
        public string Status { get; set; } = "ATIVA";
        public string? Observacoes { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public Empresa? Empresa { get; set; }
        public Banco? Banco { get; set; }
        public ICollection<LancamentoBancario> Lancamentos { get; set; } = new List<LancamentoBancario>();
    }
}
