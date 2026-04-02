using ControleFiscal.Domain.DTO.ControleFiscal;
using ControleFiscal.Domain.Model;


namespace ControleFiscal.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> LoginAsync(LoginDTO loginDto);

    }
}
