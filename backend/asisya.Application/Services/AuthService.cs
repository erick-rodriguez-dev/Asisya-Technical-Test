using asisya.Application.DTOs.Auth;
using asisya.Application.Interfaces;

namespace asisya.Application.Services;

public class AuthService : IAuthService
{
    // Credenciales fijas para demo — en producción usar Identity o tabla de usuarios
    private const string DemoUsername = "admin";
    private const string DemoPassword = "admin123";

    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    public Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        if (dto.Username != DemoUsername || dto.Password != DemoPassword)
            return Task.FromResult<TokenDto?>(null);

        var token = _jwtTokenService.GenerateToken(dto.Username);
        return Task.FromResult<TokenDto?>(new TokenDto
        {
            AccessToken = token,
            ExpiresAt = DateTime.UtcNow.AddHours(8)
        });
    }
}
