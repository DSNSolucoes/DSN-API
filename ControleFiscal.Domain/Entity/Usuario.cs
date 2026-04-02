namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class Usuario
    {
        public int Id { get; set; }
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

        public ICollection<PermissaoUsuario> PermissoesUsuarios { get; set; } = new List<PermissaoUsuario>();



    }
}
