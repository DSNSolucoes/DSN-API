namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Fornecedor
    {
        public string Id { get; set; } = string.Empty;
        public string? NmFornecedor { get; set; } = string.Empty;
        public string LojaId { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string SyncStatus { get; set; } = "PENDING";
    }
}
