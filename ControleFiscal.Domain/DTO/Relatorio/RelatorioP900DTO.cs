namespace ControleFiscal.Domain.DTO.Relatorio
{
    public partial class RelatorioP900DTO
    {
        public string? Descricao { get; set; }   
        public string? CodBarras { get; set; }   
        public string? CodReferencia { get; set; }   
        public string? Fornecedor { get; set; }
        public int? FornecedorId { get; set; }
        public decimal? Estoque { get; set; }

    }
}