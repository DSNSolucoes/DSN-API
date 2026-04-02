namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public partial class RetornoResultadoLojasDTO
    { 
        public string? loja { get; set; } 
        public decimal? ValorFiscal { get; set; }
        public decimal? ValorVendas { get; set; } 
        public  List<RetornoCFOP>? CFOP { get; set; }
    }
}