namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Permissao
    {
        public string Id { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string SyncStatus { get; set; } = "PENDING";
        public ICollection<PermissaoUsuario> PermissoesUsuarios { get; set; } = new List<PermissaoUsuario>();
    }
}
