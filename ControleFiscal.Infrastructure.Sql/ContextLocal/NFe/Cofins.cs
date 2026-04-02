using System.ComponentModel;

namespace ControleFiscal.Context.NFe
{
    public class Cofins
    {
        [Description("CST do COFINS")]
        public string CST { get; set; }

        [Description("Valor da BC do COFINS")]
        public decimal VBC { get; set; }

        [Description("Alíquota do COFINS (em percentual)")]
        public decimal PCOFINS { get; set; }

        [Description("Valor do COFINS")]
        public decimal VCOFINS { get; set; }

        [Description("Quantidade do produto na unidade tibutável")]
        public decimal QBCProd { get; set; }

        [Description("Valor do produto na unidade tibutável")]
        public decimal VAliqProd { get; set; }




    }

}
