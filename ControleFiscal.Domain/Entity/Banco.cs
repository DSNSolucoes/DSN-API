namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Banco
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Ispb { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NomeReduzido { get; set; }
        public short ParticipaCompe { get; set; } = 0;
        public string Status { get; set; } = "ATIVO";
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public ICollection<ContaBancaria> ContasBancarias { get; set; } = new List<ContaBancaria>();
    }
}
