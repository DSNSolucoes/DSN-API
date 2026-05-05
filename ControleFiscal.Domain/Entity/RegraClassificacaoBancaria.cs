namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class RegraClassificacaoBancaria
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCategoria { get; set; }
        public string PalavraChave { get; set; } = string.Empty;
        public string? TipoLancamento { get; set; }
        public int Prioridade { get; set; } = 0;
        public string Status { get; set; } = "ATIVA";
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public Empresa? Empresa { get; set; }
        public CategoriaFinanceira? Categoria { get; set; }
    }
}
