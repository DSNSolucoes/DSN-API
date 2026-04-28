namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Usuario
    {
        public string Id { get; set; } = string.Empty;
        public string? Nome { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string? Senha { get; set; } = string.Empty;
        public string? Fiscal { get; set; }
        public string? Relatorio { get; set; }
        public string? Produto { get; set; }
        public string? Financeiro { get; set; }
        public string? Bloqueado { get; set; }
        public string? Dados_Bloqueio { get; set; }
        public string? EmpresaId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string SyncStatus { get; set; } = "PENDING";
        public ICollection<PermissaoUsuario> PermissoesUsuarios { get; set; } = new List<PermissaoUsuario>();
    }
}
