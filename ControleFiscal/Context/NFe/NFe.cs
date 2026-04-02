using System.ComponentModel;
using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{
    public class NFe
    {
        [XmlElement(ElementName = "infNFe")]
        [Description("Informações de identificação da NF-e")]
        public InfNFe? InformacoesNFe { get; set; }

        public class InfNFe
        {
            [XmlElement("ide")]
            public Ide? Identificacao { get; set; }

            [Description("Emitente da NF-e")]
            [XmlElement("emit")]
            public Emitente? Emitente { get; set; }

            [XmlElement("dest")]
            [Description("Destinatário da NF-e")]
            public Destinatario? Destinatario { get; set; }

            [XmlElement("det")]
            public List<Detalhe>? Detalhe { get; set; }
        }
    }
}
