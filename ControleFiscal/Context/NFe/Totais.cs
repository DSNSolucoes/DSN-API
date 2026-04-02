using System.ComponentModel;

namespace ControleFiscal.Context.NFe
{
    public class Totais
    {
        [Description("Base de Cálculo do ICMS")]
        public decimal VBC { get; set; }

        [Description("Valor Total do ICMS")]
        public decimal VICMS { get; set; }

        [Description("Base de Cálculo do ICMS ST")]
        public decimal VBCST { get; set; }

        [Description("Valor Total do ICMS Substituição Tributária")]
        public decimal VICMSST { get; set; }

        [Description("Valor Total dos Produtos e Serviços")]
        public decimal VProd { get; set; }

        [Description("Valor Total do Frete")]
        public decimal VFrete { get; set; }

        [Description("Valor Total do Seguro")]
        public decimal VSeg { get; set; }

        [Description("Valor Total de Desconto")]
        public decimal VDesc { get; set; }

        [Description("Valor Total do II (Imposto sobre Importação)")]
        public decimal VII { get; set; }

        [Description("Valor Total do IPI")]
        public decimal VIPI { get; set; }

        [Description("Valor do PIS")]
        public decimal VPIS { get; set; }

        [Description("Valor da COFINS")]
        public decimal VCOFINS { get; set; }

        [Description("Outras Despesas acessórias")]
        public decimal VOutro { get; set; }

        [Description("Valor Total da NFe")]
        public decimal VNF { get; set; }

        [Description("Valor aproximado total de tributos federais, estaduais e municipais.")]
        public decimal VTotTrib { get; set; }





        //[XmlElement("uTrib")]
        //[Description("Valor Total do Frete")]
        //public decimal? VFrete { get; set; }

        //[XmlElement("uTrib")]
        //[Description("Valor Total do Seguro")]
        //public decimal? VSeg { get; set; }

        //[XmlElement("uTrib")]
        //[Description("Valor do Desconto")]
        //public decimal? VDesc { get; set; }

        //[XmlElement("uTrib")]
        //[Description("Valor Total do IPI")]
        //public decimal? VIPI { get; set; }

        //[XmlElement("uTrib")]
        //[Description("Valor da Outras Despesas Acessórias")]
        //public decimal? VOutro { get; set; }

        //[XmlElement("uTrib")]
        //[Description("Indica se a mercadoria é sujeita a II (Imposto de Importação)")]
        //public string IndTot { get; set; }

    }

}
