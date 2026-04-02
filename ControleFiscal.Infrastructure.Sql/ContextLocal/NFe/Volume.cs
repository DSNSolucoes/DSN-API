namespace ControleFiscal.Context.NFe
{
    using System.ComponentModel;
     
    public class Volume
    {
        [Description("Quantidade de volumes transportados")]
        public int QVol { get; set; }

        [Description("Espécie dos volumes transportados")]
        public string? Esp { get; set; }

        [Description("Marca dos volumes transportados")]
        public string? Marca { get; set; }

        [Description("Numeração dos volumes transportados")]
        public string? NVol { get; set; }

        [Description("Peso líquido (total)")]
        public decimal PesoL { get; set; }

        [Description("Peso bruto (total)")]
        public decimal PesoB { get; set; }

    }

}
