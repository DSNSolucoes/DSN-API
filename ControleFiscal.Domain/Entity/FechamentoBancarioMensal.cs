namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class FechamentoBancarioMensal
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdContaBancaria { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalDebitos { get; set; }
        public decimal SaldoFinal { get; set; }
        public int QtdLancamentos { get; set; } = 0;
        public int QtdConciliados { get; set; } = 0;
        public int QtdPendentes { get; set; } = 0;
        public string Status { get; set; } = "ABERTO";
        public int? IdFechadoPor { get; set; }
        public DateTime? DataFechamento { get; set; }
        public int? IdReabertoPor { get; set; }
        public DateTime? DataReabertura { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public Empresa? Empresa { get; set; }
        public ContaBancaria? ContaBancaria { get; set; }
    }
}
