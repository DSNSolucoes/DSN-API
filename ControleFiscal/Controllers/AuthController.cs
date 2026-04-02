using ControleFiscal.Domain.DTO.ControleFiscal; 
using ControleFiscal.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    private readonly IAuthService _AuthService;

    public AuthController(IAuthService AuthService)
    {
        _AuthService = AuthService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var tokenResponse = await _AuthService.LoginAsync(loginDto);  

        if (tokenResponse == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = tokenResponse.Token });
    }


}
