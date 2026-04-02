namespace ControleFiscal.Infrastructure.Sql.Entity
{

    public class Permissao
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;

        public ICollection<PermissaoUsuario> PermissoesUsuarios { get; set; } = new List<PermissaoUsuario>();
    }
}
