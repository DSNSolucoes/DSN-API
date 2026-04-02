using System.ComponentModel;

namespace ControleFiscal.Context.NFe
{    
    public class Transportador
    {
        [Description("CNPJ do transportador")]
        public string? CNPJ { get; set; }

        [Description("CPF do transportador")]
        public string? CPF { get; set; }

        [Description("Nome ou razão social")]
        public string? XNome { get; set; }

        [Description("Inscrição Estadual do transportador")]
        public string? IE { get; set; }

        [Description("Endereço completo")]
        public string? XEnder { get; set; }

        [Description("Nome do município")]
        public string? XMun { get; set; }

        [Description("UF do município")]
        public string? UF { get; set; }
         
    }
     

}
