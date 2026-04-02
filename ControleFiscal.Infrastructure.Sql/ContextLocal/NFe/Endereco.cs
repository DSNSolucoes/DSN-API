using System.ComponentModel;
using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{

    public class Endereco
    {
        [XmlElement("xLgr")]
        [Description("Logradouro")]
        public string? Logradouro { get; set; }

        [XmlElement("nro")]
        [Description("Número")]
        public string? Numero { get; set; }

        [XmlElement("xCpl")]
        [Description("Complemento")]
        public string? Complemento { get; set; }

        [XmlElement("xBairro")]
        [Description("Bairro")]
        public string? Bairro { get; set; }

        [XmlElement("cMun")]
        [Description("Código do município")]
        public string? CodigoMunicipio { get; set; }

        [XmlElement("xMun")]
        [Description("Nome do município")]
        public string? NomeMunicipio { get; set; }

        [XmlElement("UF")]
        [Description("UF")]
        public string? UF { get; set; }

        [XmlElement("CEP")]
        [Description("CEP")]
        public string? CEP { get; set; }

        [XmlElement("cPais")]
        [Description("Código do país")]
        public string? CodigoPais { get; set; }

        [XmlElement("xPais")]
        [Description("Nome do país")]
        public string? NomePais { get; set; }

        [XmlElement("fone")]
        [Description("Telefone")]
        public string? Telefone { get; set; }
    }

}
