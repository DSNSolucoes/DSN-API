namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class PermissaoUsuario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class PermissaoUsuario
    {
        public string Id { get; set; } = string.Empty;
        public string UsuarioId { get; set; } = string.Empty;
        public string PermissaoId { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string SyncStatus { get; set; } = "PENDING";
        public Usuario? Usuario { get; set; }
        public Permissao? Permissao { get; set; }
    }
}
