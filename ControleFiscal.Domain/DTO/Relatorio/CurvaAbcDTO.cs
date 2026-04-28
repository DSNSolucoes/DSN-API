namespace ControleFiscal.Domain.DTO.Relatorio
{
    public class CurvaAbcDTO
    {
        public int Posicao { get; set; }
        public int? CdProduto { get; set; }
        public string? CodBarras { get; set; }
        public string? NmProduto { get; set; }
        public decimal? QuantidadeVendida { get; set; }
        public decimal? ValorVendido { get; set; }
        public decimal? PercentualIndividual { get; set; }
        public decimal? PercentualAcumulado { get; set; }
        public string? Classe { get; set; } = string.Empty;
    }

    public class CurvaAbcFiltroModel
    {
        public int LojaId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }

        /// <summary>V = Valor vendido | Q = Quantidade vendida</summary>
        public string Criterio { get; set; } = "V";

        public decimal PercentualA { get; set; } = 70;
        public decimal PercentualB { get; set; } = 20;
    }

    /// <summary>Raw projection for SQL query — registered as keyless entity.</summary>
    public class CurvaAbcRaw
    {
        public int CdProduto { get; set; }
        public string? NmProduto { get; set; }
        public string? CodBarras { get; set; }
        public decimal QuantidadeVendida { get; set; }
        public decimal ValorVendido { get; set; }
    }
}
