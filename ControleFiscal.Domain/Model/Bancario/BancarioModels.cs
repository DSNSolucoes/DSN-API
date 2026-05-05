namespace ControleFiscal.Domain.Model.Bancario
{
    public class BancoSalvarModel
    {
        public string? Codigo { get; set; }
        public string? Ispb { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NomeReduzido { get; set; }
        public short ParticipaCompe { get; set; } = 0;
        public string Status { get; set; } = "ATIVO";
    }

    public class ContaBancariaSalvarModel
    {
        public int IdEmpresa { get; set; }
        public int? IdBanco { get; set; }
        public string? Agencia { get; set; }
        public string? DigitoAgencia { get; set; }
        public string? NumeroConta { get; set; }
        public string? DigitoConta { get; set; }
        public string TipoConta { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Moeda { get; set; } = "BRL";
        public decimal SaldoInicial { get; set; } = 0;
        public DateTime DataSaldoInicial { get; set; }
        public string Status { get; set; } = "ATIVA";
        public string? Observacoes { get; set; }
    }

    public class CategoriaFinanceiraSalvarModel
    {
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int? IdCategoriaPai { get; set; }
        public string Status { get; set; } = "ATIVA";
    }

    public class LancamentoBancarioSalvarModel
    {
        public int IdEmpresa { get; set; }
        public int IdContaBancaria { get; set; }
        public int? IdCategoria { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public DateTime? DataCompensacao { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string DescricaoOriginal { get; set; } = string.Empty;
        public string? DescricaoNormalizada { get; set; }
        public string? Documento { get; set; }
        public string? Observacoes { get; set; }
        public int? IdResponsavel { get; set; }
    }

    public class ConciliarModel
    {
        public int IdEmpresa { get; set; }
        public int IdLancamentoManual { get; set; }
        public int IdLancamentoImportado { get; set; }
        public string? Observacao { get; set; }
        public int? IdResponsavel { get; set; }
    }

    public class DesconciliarModel
    {
        public int IdConciliacao { get; set; }
        public string? Motivo { get; set; }
        public int? IdResponsavel { get; set; }
    }

    public class FecharMesModel
    {
        public int IdEmpresa { get; set; }
        public int IdContaBancaria { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int? IdResponsavel { get; set; }
    }

    public class RegraClassificacaoSalvarModel
    {
        public int IdEmpresa { get; set; }
        public int IdCategoria { get; set; }
        public string PalavraChave { get; set; } = string.Empty;
        public string? TipoLancamento { get; set; }
        public int Prioridade { get; set; } = 0;
        public string Status { get; set; } = "ATIVA";
    }
}
