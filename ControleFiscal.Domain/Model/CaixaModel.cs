using System;

namespace ControleFiscal.Domain.Model
{
    public class CaixaSalvarModel
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string? Descricao { get; set; }
    }

    public class CaixaMovimentacaoSalvarModel
    {
        public int Id { get; set; }
        public int CaixaId { get; set; }
        public int TipoValorCaixaId { get; set; }
        public decimal Valor { get; set; }
        public DateTime? DataCompetencia { get; set; }
        public string? Descricao { get; set; }
        public DateTime? DataRealizacao { get; set; }
        public string? AnexoNome { get; set; }
        public string? AnexoArquivo { get; set; }
        public string? AnexoContentType { get; set; }
        public string? NomeFuncionario { get; set; }
        public int? TipoValorCaixaItemId { get; set; }
    }

    public class TipoValorCaixaSalvarModel
    {
        public string? Descricao { get; set; }
    }

    public class CaixaMovimentacaoDeletarModel
    {
        public string? NomeUsuario { get; set; }
    }

    public class TipoValorCaixaItemSalvarModel
    {
        public string? Descricao { get; set; }
    }
}