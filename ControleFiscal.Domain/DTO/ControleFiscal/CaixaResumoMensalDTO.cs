namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class CaixaResumoMensalDTO
    {
        public string CaixaId { get; set; } = string.Empty;
        public string DescricaoCaixa { get; set; } = string.Empty;
        public decimal ValorTotalMes { get; set; }
    }
}
