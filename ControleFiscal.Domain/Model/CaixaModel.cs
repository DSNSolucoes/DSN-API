namespace ControleFiscal.Domain.Model
{
    public class CaixaSalvarModel
    {
        public string? Id { get; set; }
        public string LojaId { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }

    public class CaixaMovimentacaoSalvarModel
    {
        public string? Id { get; set; }
        public string CaixaId { get; set; } = string.Empty;
        public string TipoValorCaixaId { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime? DataCompetencia { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataRealizacao { get; set; }
        public string? AnexoNome { get; set; }
        public string? AnexoArquivo { get; set; }
        public string? AnexoContentType { get; set; }
        public string? NomeFuncionario { get; set; }
    }

    public class TipoValorCaixaSalvarModel
    {
        public string? Descricao { get; set; }
    }
}
