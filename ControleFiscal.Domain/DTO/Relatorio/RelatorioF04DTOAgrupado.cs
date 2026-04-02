

namespace ControleFiscal.Domain.DTO.Relatorio
{
    public partial class RelatorioF04DTOAgrupado
    {
        public int FornecedorId { get; set; }
        public string? Fornecedor { get; set; }
        public string? Unidade { get; set; }
        public decimal Quantidade { get; set; }
        public decimal SubTotal { get; set; }
    }
}