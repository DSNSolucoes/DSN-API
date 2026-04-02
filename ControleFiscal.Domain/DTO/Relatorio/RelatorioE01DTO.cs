

namespace ControleFiscal.Domain.DTO.Relatorio
{
    public partial class RelatorioE01DTO
    {
        public DateTime Emissao { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Cadastro { get; set; }
        public int FornecedorId { get; set; }
        public string Fornecedor { get; set; }
        public int NF { get; set; }
        public decimal ValorProduto { get; set; }
        public decimal ValorTotal { get; set; }
        public string Parcela { get; set; }
        public decimal ValorParcela { get; set; }
        public DateTime Vencimento { get; set; }

    }
}