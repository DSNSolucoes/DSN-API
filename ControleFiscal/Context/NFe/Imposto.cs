using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{
    public class Imposto
    {

        [XmlElement("ICMS")]
        public IcmsGrupo IcmsGrupo { get; set; }

        [XmlElement(ElementName = "IPI")]
        public Ipi? Ipi { get; set; }

        [XmlElement(ElementName = "PIS")]
        public Pis? Pis { get; set; }

        [XmlElement(ElementName = "COFINS")]
        public Cofins? Cofins { get; set; }

    }
}
