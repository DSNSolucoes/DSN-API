namespace ControleFiscal.Context.NFe
{
    using System.ComponentModel;

    public class Transporte
    {
        [Description("Modalidade do frete")]
        public string? ModFrete { get; set; }

        [Description("Informações do transportador")]
        public Transportador? Transporta { get; set; }

        [Description("Informações do veículo de transporte")]
        public Veiculo? VeicTransp { get; set; }

        [Description("Informações de reboques do veículo")]
        public List<Veiculo>? Reboque { get; set; }

        [Description("Identificação da balsa")]
        public string? Balsa { get; set; }

        [Description("Informações dos volumes transportados")]
        public List<Volume>? Vol { get; set; }
    }

    
}
