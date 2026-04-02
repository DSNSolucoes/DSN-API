namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public partial class RecuperarUsuarioDTO : LoginDTO
    {      
        public string? ConfimarSenha { get; set; }
        public string? NovaSenha { get; set; }
    }
}