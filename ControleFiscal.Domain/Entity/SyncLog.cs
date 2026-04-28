namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class SyncLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("D");
        public string Tabela { get; set; } = string.Empty;
        public string RegistroId { get; set; } = string.Empty;
        public string Operacao { get; set; } = string.Empty;   // INSERT | UPDATE | DELETE
        public string Status { get; set; } = "PENDING";         // PENDING | SYNCED | ERRO
        public string? Payload { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SyncedAt { get; set; }
    }
}
