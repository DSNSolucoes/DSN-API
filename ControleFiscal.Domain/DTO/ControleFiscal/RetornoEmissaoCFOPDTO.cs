namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class RetornoEmissaoCFOPDTO
    {
        public int numNfce { get; set; }
        public int serieNfce { get; set; }
        public int numTerminal { get; set; }
        public int? numDocumento { get; set; }
        public string? ChaveNfce { get; set; }
        public int? numItens { get; set; }
        public decimal? vlTotalNfce { get; set; }
        public int cdEmpresa { get; set; }
        public int? cdCertificado { get; set; }
        public DateTime? dtCadastro { get; set; }
        public string tipoEmissao { get; set; }
        public DateTime? dtEmissao { get; set; }
        public DateTime? contingenciaDatahora { get; set; }
        public string? Ambiente { get; set; }
        public DateTime? contingenciaEnviando { get; set; }
        public decimal valor { get; set; }
        public string? CFOP { get; set; }

    }
}
