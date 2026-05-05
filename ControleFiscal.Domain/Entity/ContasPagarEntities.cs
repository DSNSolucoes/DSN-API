namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class CategoriaContaPagar
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? IdCategoriaPai { get; set; }
        public string Status { get; set; } = "ATIVA";
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public Empresa? Empresa { get; set; }
        public CategoriaContaPagar? CategoriaPai { get; set; }
        public ICollection<ContaPagar> ContasPagar { get; set; } = new List<ContaPagar>();
    }

    public class CentroCustoCP
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public string Status { get; set; } = "ATIVO";
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public Empresa? Empresa { get; set; }
        public ICollection<ContaPagar> ContasPagar { get; set; } = new List<ContaPagar>();
    }

    public class ContaPagar
    {
        public int Id { get; set; }
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
        public decimal ValorTotal { get; set; }
        public decimal ValorPago { get; set; } = 0;
        public decimal SaldoAPagar { get; set; }
        public int NumeroParcela { get; set; } = 1;
        public int TotalParcelas { get; set; } = 1;
        public int? IdContaOrigem { get; set; }
        public short Recorrente { get; set; } = 0;
        public int? IdRecorrencia { get; set; }
        public string Status { get; set; } = "ABERTA";
        public string? Observacoes { get; set; }
        public string? MotivoCancelamento { get; set; }
        public int? IdResponsavelCriacao { get; set; }
        public int? IdResponsavelAtualizacao { get; set; }
        public int? IdResponsavelCancelamento { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataCancelamento { get; set; }

        public Empresa? Empresa { get; set; }
        public CategoriaContaPagar? Categoria { get; set; }
        public CentroCustoCP? CentroCusto { get; set; }
        public ICollection<PagamentoContaPagar> Pagamentos { get; set; } = new List<PagamentoContaPagar>();
    }

    public class PagamentoContaPagar
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdContaPagar { get; set; }
        public DateTime DataPagamento { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorDesconto { get; set; } = 0;
        public decimal ValorJuros { get; set; } = 0;
        public decimal ValorMulta { get; set; } = 0;
        public string? FormaPagamento { get; set; }
        public string? DocumentoPagamento { get; set; }
        public string? Comprovante { get; set; }
        public string Status { get; set; } = "ATIVO";
        public string? Observacoes { get; set; }
        public int? IdResponsavel { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public int? IdResponsavelEstorno { get; set; }
        public DateTime? DataEstorno { get; set; }
        public string? MotivoEstorno { get; set; }

        public Empresa? Empresa { get; set; }
        public ContaPagar? ContaPagar { get; set; }
    }

    public class ContaPagarRecorrente
    {
        public int Id { get; set; }
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
        public string Status { get; set; } = "ATIVA";
        public string? Observacoes { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime? DataAtualizacao { get; set; }

        public Empresa? Empresa { get; set; }
        public CategoriaContaPagar? Categoria { get; set; }
        public CentroCustoCP? CentroCusto { get; set; }
    }

    public class AuditoriaContasPagar
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdResponsavel { get; set; }
        public string Entidade { get; set; } = string.Empty;
        public int? IdEntidade { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string? DadosAnteriores { get; set; }
        public string? DadosNovos { get; set; }
        public string? Ip { get; set; }
        public string? UserAgent { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public Empresa? Empresa { get; set; }
    }
}
