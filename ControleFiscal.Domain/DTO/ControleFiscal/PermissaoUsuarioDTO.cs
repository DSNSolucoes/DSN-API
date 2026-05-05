namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public class PermissaoUsuarioDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int PermissaoId { get; set; } 
        public PermissaoDTO? Permissao { get; set; }
    }
}
