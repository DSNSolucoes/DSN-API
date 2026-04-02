namespace ControleFiscal.Domain.DTO.ControleFiscal
{

    public class CaixaMovimentacoesDTO
    {
        public int Id { get; set; }
        public int CaixaId { get; set; }
        public int TipoValorCaixaId { get; set; }

        // total do tipo no mês
        public decimal ValorTotal { get; set; }

        public TipoValor TipoValorCaixa { get; set; }

        // um item por dia, com ou sem movimentação
        public List<CaixaMovimentacaoDetalhesDTO> Detalhes { get; set; } = new();
    }


}