namespace ControleFiscal.Domain.DTO.ContasPagar
{
    public class FornecedorDTO
    {
        public int CdFornecedor { get; set; }
        public long IdEmpresa { get; set; }
        public string? Nome { get; set; }
        public string? NomeFantasia { get; set; }
        public string? Documento { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Status { get; set; }
    }

    public class CategoriaContaPagarDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? IdCategoriaPai { get; set; }
        public string? NomeCategoriaPai { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class CentroCustoCPDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ContaPagarDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdFornecedor { get; set; }
        public string? NomeFornecedor { get; set; }
        public int? IdCategoria { get; set; }
        public string? NomeCategoria { get; set; }
        public int? IdCentroCusto { get; set; }
        public string? NomeCentroCusto { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? NumeroDocumento { get; set; }
        public string? TipoDocumento { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataCompetencia { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorAcrescimo { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorPago { get; set; }
        public decimal SaldoAPagar { get; set; }
        public int NumeroParcela { get; set; }
        public int TotalParcelas { get; set; }
        public int? IdContaOrigem { get; set; }
        public short Recorrente { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public DateTime DataCriacao { get; set; }
        public int DiasAtraso { get; set; }
    }

    public class PagamentoContaPagarDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdContaPagar { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorJuros { get; set; }
        public decimal ValorMulta { get; set; }
        public string? FormaPagamento { get; set; }
        public string? DocumentoPagamento { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataEstorno { get; set; }
        public string? MotivoEstorno { get; set; }
    }

    public class ContaPagarRecorrenteDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdFornecedor { get; set; }
        public string? NomeFornecedor { get; set; }
        public int? IdCategoria { get; set; }
        public string? NomeCategoria { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Periodicidade { get; set; } = string.Empty;
        public int? DiaVencimento { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class DashboardContasPagarDTO
    {
        public decimal TotalEmAberto { get; set; }
        public decimal TotalVencido { get; set; }
        public decimal TotalAVencer { get; set; }
        public decimal TotalPagoMes { get; set; }
        public int QtdVencidas { get; set; }
        public int QtdAVencer { get; set; }
        public List<ResumoFornecedorDTO> MaioresFornecedores { get; set; } = new();
        public List<ResumoCategoriaDTO> PorCategoria { get; set; } = new();
    }

    public class ResumoFornecedorDTO
    {
        public string NomeFornecedor { get; set; } = string.Empty;
        public int QtdContas { get; set; }
        public decimal TotalEmAberto { get; set; }
        public decimal TotalPago { get; set; }
    }

    public class ResumoCategoriaDTO
    {
        public string NomeCategoria { get; set; } = string.Empty;
        public int QtdContas { get; set; }
        public decimal TotalEmAberto { get; set; }
        public decimal TotalPago { get; set; }
    }
}
