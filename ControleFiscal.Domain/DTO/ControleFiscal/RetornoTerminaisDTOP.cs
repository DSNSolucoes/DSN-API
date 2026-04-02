namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public partial class RetornoTerminaisDTO
    {
        public short NumTerminal { get; set; }
        public string? Descricao { get; set; }
        public bool EmissaoExtraordinaria  { get; set; }
        public bool EmissaoExtraordinariaicms  { get; set; }
        public string? EmissaoExtraordinariancm  { get; set; }
        public decimal? ValorFiscal { get; set; }
        public decimal? ValorVendas { get; set; }
        public string? NCM { get; set; }
        public  List<RetornoCFOPTerminal>? cfop { get; set; }
    }
}