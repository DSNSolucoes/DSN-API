namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Fornecedor
    {
        public int CdFornecedor { get; set; }
        public string? NmFornecedor { get; set; } = string.Empty;
        public long idLoja { get; set; } 

    }
}
