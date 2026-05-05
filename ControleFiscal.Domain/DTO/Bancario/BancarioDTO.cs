namespace ControleFiscal.Domain.DTO.Bancario
{
    public class BancoDTO
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Ispb { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NomeReduzido { get; set; }
        public short ParticipaCompe { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ContaBancariaDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdBanco { get; set; }
        public string? NomeBanco { get; set; }
        public string? CodigoBanco { get; set; }
        public string? Agencia { get; set; }
        public string? DigitoAgencia { get; set; }
        public string? NumeroConta { get; set; }
        public string? DigitoConta { get; set; }
        public string TipoConta { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Moeda { get; set; } = "BRL";
        public decimal SaldoInicial { get; set; }
        public DateTime DataSaldoInicial { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Observacoes { get; set; }
    }

    public class CategoriaFinanceiraDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int? IdCategoriaPai { get; set; }
        public string? NomeCategoriaPai { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class LancamentoBancarioDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdContaBancaria { get; set; }
        public string? NomeConta { get; set; }
        public int? IdCategoria { get; set; }
        public string? NomeCategoria { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public DateTime? DataCompensacao { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string DescricaoOriginal { get; set; } = string.Empty;
        public string? DescricaoNormalizada { get; set; }
        public string? Documento { get; set; }
        public string Origem { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Fitid { get; set; }
        public string? Observacoes { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class SaldoContaDTO
    {
        public int IdContaBancaria { get; set; }
        public string NomeConta { get; set; } = string.Empty;
        public decimal SaldoInicial { get; set; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalDebitos { get; set; }
        public decimal SaldoAtual { get; set; }
    }

    public class SaldoEmpresaDTO
    {
        public int IdEmpresa { get; set; }
        public decimal SaldoTotal { get; set; }
        public List<SaldoContaDTO> Contas { get; set; } = new();
    }

    public class FechamentoBancarioDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdContaBancaria { get; set; }
        public string? NomeConta { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal TotalCreditos { get; set; }
        public decimal TotalDebitos { get; set; }
        public decimal SaldoFinal { get; set; }
        public int QtdLancamentos { get; set; }
        public int QtdConciliados { get; set; }
        public int QtdPendentes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DataFechamento { get; set; }
    }

    public class ConciliacaoBancariaDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int? IdLancamentoManual { get; set; }
        public int? IdLancamentoImportado { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Observacao { get; set; }
        public DateTime DataCriacao { get; set; }
    }

    public class ArquivoBancarioDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdContaBancaria { get; set; }
        public string NomeOriginal { get; set; } = string.Empty;
        public string Formato { get; set; } = string.Empty;
        public int? TamanhoBytes { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalLidos { get; set; }
        public int TotalImportados { get; set; }
        public int TotalDuplicados { get; set; }
        public int TotalErros { get; set; }
        public string? LogErro { get; set; }
        public DateTime DataUpload { get; set; }
        public DateTime? DataProcessamento { get; set; }
    }

    public class RegraClassificacaoDTO
    {
        public int Id { get; set; }
        public int IdEmpresa { get; set; }
        public int IdCategoria { get; set; }
        public string? NomeCategoria { get; set; }
        public string PalavraChave { get; set; } = string.Empty;
        public string? TipoLancamento { get; set; }
        public int Prioridade { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class DashboardBancarioDTO
    {
        public decimal SaldoTotalEmpresa { get; set; }
        public decimal CreditosMes { get; set; }
        public decimal DebitosMes { get; set; }
        public decimal ResultadoLiquidoMes { get; set; }
        public int LancamentosPendentesConciliacao { get; set; }
        public List<SaldoContaDTO> SaldoPorConta { get; set; } = new();
    }
}
