
using ControleFiscal.Infrastructure.Sql.Entity;
using System.Text;

namespace ControleFiscal.Domain.DTO.ControleFiscal
{
    public partial class LoginRetornoDTO : LoginDTO
    {
        public LoginRetornoDTO(Usuario usuario) 
        {
            Id = usuario.Id;
            Login = usuario.Login; 
            Nome = usuario.Nome;    
            Fiscal = usuario.Fiscal;    
            Financeiro = usuario.Financeiro;
            Produto = usuario.Produto;
            Relatorio = usuario.Relatorio;
            Senha = LoginRetornoDTO.RetornarSenha();
            CriadoEm = Convert.ToBase64String(Encoding.UTF8.GetBytes(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
        }
        public int Id { get; set; }
        public string? Nome { get; set; } = string.Empty; 
        public string? Fiscal { get; set; }
        public string? Relatorio { get; set; }
        public string? Produto { get; set; }
        public string? Financeiro { get; set; }
        

    }
}