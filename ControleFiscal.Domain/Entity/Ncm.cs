namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Ncm
    {
        public int Id { get; set; }
        public string? NCM { get; set; }
        public string? Padrao { get; set; }
namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Ncm
    {
        public string Id { get; set; } = string.Empty;
        public string? NCM { get; set; }
        public string? Padrao { get; set; }
        public string? Descricao { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string SyncStatus { get; set; } = "PENDING";
    }
}
