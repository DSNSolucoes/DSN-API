namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class CaixaResumoMensalDTO
    {
        public int CaixaId { get; set; }
        public string? DescricaoCaixa { get; set; }
        public decimal ValorTotalMes { get; set; }
    }
}