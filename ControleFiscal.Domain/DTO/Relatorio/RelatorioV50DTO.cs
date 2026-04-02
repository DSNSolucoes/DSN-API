 

namespace ControleFiscal.Domain.DTO.Relatorio
{
    public partial class RelatorioV50DTO
    {
        public string? codBarras { get; set; }
        public string? nomeProduto { get; set; }
        public string? codProdFornec { get; set; }
        public double? valorUnitario { get; set; }
        public double? ipi { get; set; }
        public double? st { get; set; }
        public double? precoCusto { get; set; }
        public DateTime? dataUltimaCompra { get; set; }
        public double? qtdUltimaCompra { get; set; }
        public double? quantidadeTotal { get; set; }
        public double? precoMedio { get; set; }
        public double? estoqueAtual { get; set; }
        public double? valorTotal { get; set; }
        public double? markup { get; set; }
        public double? quantidadecomprada { get; set; }
        public double? percentualvendido { get; set; }
        public string?  Loja { get; set; } 
        public double? undConversao { get; set; }
        public decimal? mediaVenda { get; set; }

    }
}