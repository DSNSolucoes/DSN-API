namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public partial class LoginDTO
    {
        private string? _login;

        public string? Login
        {
            get => _login?.ToUpper();
            set => _login = value?.ToUpper();
        }

        public string? Senha { get; set; }
        public string? CriadoEm { get; set; }
        public static string RetornarSenha() => "DG26$02&25"; 
        public static bool TentativaDeInvasao(string senha) => senha == RetornarSenha();


        public  bool IsValid()
        {
            Login = Login?.Trim();
           if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Senha))
           {
             return false;
           }

                return true;
        }
    }
    
}