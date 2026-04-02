using System.ComponentModel;
using System.Xml.Serialization;

namespace ControleFiscal.Context.NFe
{
    public class Emitente
    {
        [Description("CNPJ do emitente")]
        public string? CNPJ { get; set; }

        [Description("CPF do emitente")]
        public string? CPF { get; set; }

        [Description("Nome do emitente")]
        public string? XNome { get; set; }

        [Description("Nome fantasia do emitente")]
        public string? XFant { get; set; }

        [Description("Inscrição Estadual do emitente")]
        public string? IE { get; set; }

        [Description("Inscrição Estadual do Substituto Tributário")]
        public string? IEST { get; set; }

        [Description("Inscrição Municipal do Prestador de Serviço")]
        public string? IM { get; set; }

        [Description("CNAE fiscal")]
        public string? CNAE { get; set; }

        [Description("Código de Regime Tributário")]
        public string? CRT { get; set; }


        [XmlElement("enderDest")]
        [Description("Endereço do emitente")]
        public Endereco Endereco { get; set; }
    }
}