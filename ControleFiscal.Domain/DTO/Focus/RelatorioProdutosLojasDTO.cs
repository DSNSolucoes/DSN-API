namespace ControleFiscal.Domain.DTO.Focus
{
    public partial class RelatorioProdutosLojasDTO
    {
        public string? CodBarras { get; set; }
      
        public string? Descricao { get; set; }
        public string? NomeLoja { get; set; }

        public decimal PrecoCusto { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal EstoqueAtual { get; set; }

    }
}