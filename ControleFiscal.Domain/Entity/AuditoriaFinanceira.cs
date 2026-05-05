namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class AuditoriaFinanceira
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdResponsavel { get; set; }
        public string Entidade { get; set; } = string.Empty;
        public int? IdEntidade { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string? DadosAnteriores { get; set; }
        public string? DadosNovos { get; set; }
        public string? Ip { get; set; }
        public string? UserAgent { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public Empresa? Empresa { get; set; }
    }
}
