using System.ComponentModel;
using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{
    public class Detalhe
    {
        [XmlAttribute("nItem")]
        public int nItem { get; set; }

        [XmlElement("prod")]
        [Description("Produtos e serviços da NF-e")]
        public Produto? Produto { get; set; }

        [XmlElement("imposto")]
        [Description("Imposto")]
        public Imposto? Imposto { get; set; }
    }
}
