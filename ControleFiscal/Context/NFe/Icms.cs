using System.ComponentModel;
using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{
    public class Icms
    {
        [XmlElement("orig")]
        [Description("Origem da mercadoria")]
        public string? Origem { get; set; }

        [Description("CST do ICMS")]
        public string? CST { get; set; }

        [Description("Modalidade de determinação da BC do ICMS")]
        public string? ModBC { get; set; }

        [Description("Valor da BC do ICMS")]
        public decimal VBC { get; set; }

        [Description("Alíquota do ICMS")]
        public decimal PICMS { get; set; }

        [Description("Valor do ICMS")]
        public decimal VICMS { get; set; }

    }

    public class Icms10
    {
        [XmlElement("orig")]
        [Description("Origem da mercadoria")]
        public string? Origem { get; set; }

        [Description("CST do ICMS")]
        public string? CST { get; set; }

        [Description("Modalidade de determinação da BC do ICMS")]
        public string? ModBC { get; set; }

        [Description("Valor da BC do ICMS")]
        public decimal VBC { get; set; }

        [Description("Alíquota do ICMS")]
        public decimal PICMS { get; set; }

        [Description("Valor do ICMS")]
        public decimal VICMS { get; set; }

    }

}
