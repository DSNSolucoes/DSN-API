using System.ComponentModel;

namespace ControleFiscal.Context.NFe
{
    public class Ipi
    {
        [Description("CNPJ do produtor da mercadoria, quando diferente do emitente")]
        public string? CNPJProd { get; set; }

        [Description("Código do selo de controle IPI")]
        public string? CSelo { get; set; }

        [Description("Quantidade de selo de controle")]
        public int? QSelo { get; set; }

        [Description("Código de Enquadramento Legal do IPI")]
        public string? CEnq { get; set; }

        [Description("CST do IPI")]
        public string? CST { get; set; }

        [Description("Valor da BC do IPI")]
        public decimal? VBC { get; set; }

        [Description("Alíquota do IPI")]
        public decimal? PIPI { get; set; }

        [Description("Valor do IPI")]
        public decimal? VIPI { get; set; }
    }

}
