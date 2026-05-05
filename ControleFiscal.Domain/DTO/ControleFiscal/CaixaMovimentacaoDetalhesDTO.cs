namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class CaixaMovimentacaoDetalhesDTO
    {
        public int Id { get; set; }
        public int TipoValorCaixaId { get; set; }
        public decimal Valor { get; set; }

        public string Descricao { get; set; }

        public DateTime? DataCompetencia { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DataRealizacao { get; set; }

        public int AnoCompetencia { get; set; }
        public int MesCompetencia { get; set; }
        public int DiaCompetencia { get; set; }

        public string NomeFuncionario { get; set; }

        public int? TipoValorItemId { get; set; }
        public string TipoValorItemDescricao { get; set; }

        public string AnexoNome { get; set; }
        public string AnexoArquivo { get; set; }
        public string AnexoContentType { get; set; }
    }
}