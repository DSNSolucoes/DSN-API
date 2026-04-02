
namespace ControleFiscal.Domain.DTO.Relatorio
{
    public class RelatorioF04DTO
    {
        public string? CodigoBarras { get; set; } 
        public string? NomeProduto { get; set; }  
        public decimal Quantidade { get; set; }  
        public decimal Preco { get; set; }  
        public decimal SubTotal { get; set; }  
        public string? Unidade { get; set; }  
    }
}