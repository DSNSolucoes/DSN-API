using System.ComponentModel;
using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{
    public class Produto
    {
        [XmlElement("cProd")]
        [Description("Código do produto")]
        public string? CodigoProduto { get; set; }

        [XmlElement("cEAN")]
        [Description("Código EAN do produto")]
        public string? Ean { get; set; }

        [XmlElement("xProd")]
        [Description("Descrição do produto")]
        public string? Descricao { get; set; }

        [XmlElement("NCM")]
        [Description("Código NCM do produto")]
        public string? NCM { get; set; }

        [XmlElement("CEST")]
        [Description("Código EX da TIPI")]
        public string? CEST { get; set; }

        [XmlElement("CFOP")]
        [Description("Código Fiscal de Operações e Prestações")]
        public string? CFOP { get; set; }

        [XmlElement("uCom")]
        [Description("Unidade Comercial do produto")]
        public string? UnidadeComercial { get; set; }

        [XmlElement("qCom")]
        [Description("Quantidade Comercial do produto")]
        public decimal QuantidadeComercial { get; set; }

        [XmlElement("vUnCom")]
        [Description("Valor Unitário de Comercialização")]
        public decimal ValorUnitarioComercializacao { get; set; }

        [XmlElement("vProd")]
        [Description("Valor Total Bruto dos Produtos ou Serviços")]
        public decimal ValorBrutoProdutoServico { get; set; }

        [XmlElement("cEANTrib")]
        [Description("Código EAN da Unidade Tributável")]
        public string? EANTributavel { get; set; }

        [XmlElement("uTrib")]
        [Description("Unidade Tributável do produto")]
        public string? UnidadeTributavel { get; set; }

        [XmlElement("qTrib")]
        [Description("Quantidade Tributável do produto")]
        public decimal QuantidadeTributavel { get; set; }

        [XmlElement("vUnTrib")]
        [Description("Valor Unitário de tributação")]
        public decimal ValorUnitarioTributacao { get; set; }



    }

}
