namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class CategoriaFinanceira
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // CREDITO | DEBITO | AMBOS
        public int? IdCategoriaPai { get; set; }
        public string Status { get; set; } = "ATIVA";
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public Empresa? Empresa { get; set; }
        public CategoriaFinanceira? CategoriaPai { get; set; }
        public ICollection<CategoriaFinanceira> SubCategorias { get; set; } = new List<CategoriaFinanceira>();
    }
}
