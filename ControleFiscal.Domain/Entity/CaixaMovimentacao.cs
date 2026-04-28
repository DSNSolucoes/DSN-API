#nullable disable

namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public partial class CaixaMovimentacao
    {
        public string Id { get; set; } = string.Empty;
        public string CaixaId { get; set; } = string.Empty;
        public string TipoValorCaixaId { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string Ativo { get; set; }
        public DateTime? DataCompetencia { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataRealizacao { get; set; }
        public string AnexoNome { get; set; }
        public string AnexoArquivo { get; set; }
        public string AnexoContentType { get; set; }
        public string NomeFuncionario { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string SyncStatus { get; set; } = "PENDING";
    }
}