namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Fornecedor
    {
        public int CdFornecedor { get; set; }
        public string? NmFornecedor { get; set; } = string.Empty;
        public long Id_Empresa { get; set; }

        // Campos adicionados para o módulo Contas a Pagar
        public string? NomeFantasia { get; set; }
        public string? Documento { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Observacoes { get; set; }
        public string? Status { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
