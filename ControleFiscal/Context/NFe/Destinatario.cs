using System.ComponentModel;
using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{
    public class Destinatario
    {
        [XmlElement("CNPJ")]
        [Description("CNPJ do destinatário")]
        public string? CNPJ { get; set; }

        [XmlElement("CPF")]
        [Description("CPF do destinatário")]
        public string? CPF { get; set; }

        [XmlElement("xNome")]
        [Description("Nome do destinatário")]
        public string? Nome { get; set; }

        [XmlElement("indIEDest")]
        [Description("Indicador da IE do destinatário")]
        public string? IndIEDest { get; set; }

        [XmlElement("IE")]
        [Description("Inscrição Estadual do destinatário")]
        public string? IE { get; set; }

        [XmlElement("ISUF")]
        [Description("Inscrição SUFRAMA do destinatário")]
        public string? ISUF { get; set; }

        [XmlElement("email")]
        [Description("E-mail do destinatário")]
        public string? Email { get; set; }

        [Description("Endereço do destinatário")]
        [XmlElement("enderDest")]
        public Endereco Endereco { get; set; }
    }

}
