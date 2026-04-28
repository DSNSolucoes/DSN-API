namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class CaixaMovimentacaoDetalhesDTO
    {
        public string Id { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataCompetencia { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataRealizacao { get; set; }
        public int AnoCompetencia { get; set; }
        public int MesCompetencia { get; set; }
        public int DiaCompetencia { get; set; }
        public string NomeFuncionario { get; set; } = string.Empty;
        public string? AnexoNome { get; set; }
        public string? AnexoArquivo { get; set; }
        public string? AnexoContentType { get; set; }
    }
}
