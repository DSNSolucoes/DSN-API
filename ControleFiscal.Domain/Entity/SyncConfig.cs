namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class SyncConfig
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("D");
        public DateTime? UltimoSync { get; set; }
        public bool EmSincronizacao { get; set; } = false;
        public string? UrlNuvem { get; set; }
    }
}
