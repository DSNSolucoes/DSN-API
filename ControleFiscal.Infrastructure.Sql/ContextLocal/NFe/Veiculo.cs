namespace ControleFiscal.Context.NFe
{
    using System.ComponentModel;
     
    public class Veiculo
    {
        [Description("Placa do veículo")]
        public string? Placa { get; set; }

        [Description("UF da placa do veículo")]
        public string? UF { get; set; }

        [Description("Registro Nacional de Transportador de Carga (ANTT)")]
        public string? RNTC { get; set; }
    }
     

}
