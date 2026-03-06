using asisya.Application.DTOs.Auth;

namespace asisya.Application.Interfaces;

public interface IAuthService
{
    Task<TokenDto?> LoginAsync(LoginDto dto);
}
