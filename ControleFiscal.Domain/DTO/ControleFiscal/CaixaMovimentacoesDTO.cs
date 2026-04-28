namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class CaixaMovimentacoesDTO
    {
        public string Id { get; set; } = string.Empty;
        public string CaixaId { get; set; } = string.Empty;
        public string TipoValorCaixaId { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
        public TipoValor TipoValorCaixa { get; set; } = new();
        public List<CaixaMovimentacaoDetalhesDTO> Detalhes { get; set; } = new();
    }
}
