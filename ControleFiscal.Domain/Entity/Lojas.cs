namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Lojas
    {
        public int Id { get; set; }
        public int Porta { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Caminho { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string CNPJ { get; set; } = string.Empty;
        public double PercentualST { get; set; } = 30;

    }
}
