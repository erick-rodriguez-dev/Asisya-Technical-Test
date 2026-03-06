namespace asisya.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(string username);
}
