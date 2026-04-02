#nullable disable

namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public partial class CaixaMovimentacao
    {
        public int Id { get; set; }
        public int CaixaId { get; set; }
        public int TipoValorCaixaId { get; set; }
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
    }
}