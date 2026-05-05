namespace ControleFiscal.Domain.Model.ContasPagar
{
    public class FornecedorSalvarModel
    {
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;   // → NM_FORNECEDOR
        public string? NomeFantasia { get; set; }
        public string? Documento { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Observacoes { get; set; }
        public string Status { get; set; } = "ATIVO";
    }

    public class CategoriaContaPagarSalvarModel
    {
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? IdCategoriaPai { get; set; }
        public string Status { get; set; } = "ATIVA";
    }

    public class CentroCustoCPSalvarModel
    {
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public string Status { get; set; } = "ATIVO";
    }

    public class ContaPagarSalvarModel
    {
        public int IdEmpresa { get; set; }
        public int? IdFornecedor { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdCentroCusto { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? NumeroDocumento { get; set; }
        public string? SerieDocumento { get; set; }
        public string? TipoDocumento { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime? DataCompetencia { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorOriginal { get; set; }
        public decimal ValorDesconto { get; set; } = 0;
        public decimal ValorAcrescimo { get; set; } = 0;
        public decimal ValorMulta { get; set; } = 0;
        public decimal ValorJuros { get; set; } = 0;
        public string? Observacoes { get; set; }
        public int? IdResponsavel { get; set; }
    }

    public class GerarParcelasModel
    {
        public int IdEmpresa { get; set; }
        public int? IdFornecedor { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdCentroCusto { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string? NumeroDocumento { get; set; }
        public string? TipoDocumento { get; set; }
        public DateTime? DataEmissao { get; set; }
        public DateTime DataPrimeiroVencimento { get; set; }
        public int NumeroParcelas { get; set; } = 2;
        public decimal ValorTotal { get; set; }
        public string? Observacoes { get; set; }
        public int? IdResponsavel { get; set; }
    }

    public class CancelarContaModel
    {
        public int IdEmpresa { get; set; }
        public string MotivoCancelamento { get; set; } = string.Empty;
        public int? IdResponsavel { get; set; }
    }

    public class RegistrarPagamentoModel
    {
        public int IdEmpresa { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorDesconto { get; set; } = 0;
        public decimal ValorJuros { get; set; } = 0;
        public decimal ValorMulta { get; set; } = 0;
        public string? FormaPagamento { get; set; }
        public string? DocumentoPagamento { get; set; }
        public string? Observacoes { get; set; }
        public int? IdResponsavel { get; set; }
    }

    public class EstornarPagamentoModel
    {
        public int IdEmpresa { get; set; }
        public string MotivoEstorno { get; set; } = string.Empty;
        public int? IdResponsavel { get; set; }
    }

    public class ReobrirContaModel
    {
        public int IdEmpresa { get; set; }
        public int? IdResponsavel { get; set; }
    }

    public class ContaPagarRecorrenteSalvarModel
    {
        public int IdEmpresa { get; set; }
        public int? IdFornecedor { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdCentroCusto { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Periodicidade { get; set; } = "MENSAL";
        public int? DiaVencimento { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? Observacoes { get; set; }
    }

    public class GerarContasRecorrentesModel
    {
        public int IdEmpresa { get; set; }
        public DateTime DataLimite { get; set; }
        public int? IdResponsavel { get; set; }
    }

    public class FiltroContasPagarModel
    {
        public int IdEmpresa { get; set; }
        public int? IdFornecedor { get; set; }
        public int? IdCategoria { get; set; }
        public int? IdCentroCusto { get; set; }
        public string? Status { get; set; }
        public DateTime? VencimentoDe { get; set; }
        public DateTime? VencimentoAte { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? Descricao { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 50;
    }
}
