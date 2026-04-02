 

namespace ControleFiscal.Domain.DTO.Relatorio
{
    public partial class RelatorioE03DTO
    {
        public int Sequencial { get; set; }
        public string? CodigoBarras { get; set; }
        public string? Unidade { get; set; }
        public string? NomeProduto { get; set; }
        public decimal PrecoCusto { get; set; }
        public decimal MargemLucro { get; set; }
        public decimal PrecoVenda { get; set; }
        public decimal Quantidade { get; set; }
        public decimal UnidadeConversao { get; set; }
        public decimal ValorTotal { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? CodigoFornecedor { get; set; }
        public string? NomeFornecedor { get; set; }
        public string? Cnpj { get; set; }
        public string? Cfop { get; set; }
        public decimal DescontoTotal { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime UltimaAlteracao { get; set; }
        public decimal IcmsBase { get; set; }
        public decimal IcmsValor { get; set; }
        public decimal IcmsBaseSubstituicao { get; set; }
        public decimal IcmsValorSubstituicao { get; set; }
        public decimal ValorTotalProdutos { get; set; }
        public decimal ValorFrete { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal OutrasDespesas { get; set; }
        public decimal ValorIpi { get; set; }
        public decimal ValorTotalNota { get; set; }
        public string? ClassificacaoFiscal { get; set; }
        public string? Cst { get; set; }

    }
}