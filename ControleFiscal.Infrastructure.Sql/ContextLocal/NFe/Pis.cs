using System.ComponentModel;

namespace ControleFiscal.Context.NFe
{
    public class Pis
    {
        [Description("CST do PIS")]
        public string? CST { get; set; }

        [Description("Valor da BC do PIS")]
        public decimal? VBC { get; set; }

        [Description("Alíquota do PIS (em percentual)")]
        public decimal? PPIS { get; set; }

        [Description("Valor do PIS")]
        public decimal? VPIS { get; set; }

        [Description("Quantidade do produto na unidade tibutável")]
        public decimal? QBCProd { get; set; }

        [Description("Quantidade do produto na unidade tibutável")]
        public decimal? VAliqProd { get; set; }
    }
}
