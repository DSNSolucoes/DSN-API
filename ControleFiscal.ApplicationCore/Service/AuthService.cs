using System.Security.Claims;
using System.Text;
using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Services.Interfaces;
using Microsoft.Extensions.Logging;
using ControleFiscal.Domain.Model; 
using ControleFiscal.Infrastructure.Sql.Focus.Context;
using ControleFiscal.Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ControleFiscal.ApplicationCore.Service
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly ContextControleFiscalContext _Context;
        private readonly ContextLocalContext _ContextLocal;

        public AuthService(ILogger<AuthService> logger, ContextControleFiscalContext Context, ContextLocalContext ContextLocal)
        {
            _logger = logger;
            _Context = Context;
            _ContextLocal = ContextLocal;
        }

        public Task<TokenResponse> LoginAsync(LoginDTO loginDto)
        {
            if (loginDto.IsValid())
            {
                var ListaUsuarios = _ContextLocal.Logins.ToList();

                var user = ListaUsuarios.FirstOrDefault(x => x.Login.ToUpper() == loginDto.Login && x.Senha == loginDto.Senha);

                if (user != null)
                {
                    var listaPermissao = _ContextLocal.PermissoesUsuarios
                        .Include(x => x.Permissao)
                        .Select(x => x.Permissao.Nome)
                        .ToList();

                    var token = GenerateToken(user.Login, listaPermissao);
                    return Task.FromResult(new TokenResponse { Token = token });
                }

            }


            return Task.FromResult(new TokenResponse());
        }

        public string GenerateToken(string username, List<string> permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kjkszpjuzumymwanrltwpdnejohhesoyamjumpjet"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                issuer: "yourdomain.com",
                audience: "yourdomain.com",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateJwtToken(string username)
        {
            var key = Encoding.ASCII.GetBytes("kjkszpjuzumymwanrltwpdnejohhesoyamjumpjet");
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}