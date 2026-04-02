namespace ControleFiscal.Infrastructure.Sql.Entity
{
    public class PermissaoUsuario
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int PermissaoId { get; set; }

        public Usuario? Usuario { get; set; }
        public Permissao? Permissao { get; set; }
    }
}
