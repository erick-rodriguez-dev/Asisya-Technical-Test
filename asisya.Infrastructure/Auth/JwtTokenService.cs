using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using asisya.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace asisya.Infrastructure.Auth;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiresHours;

    public JwtTokenService(IConfiguration configuration)
    {
        _secret   = configuration["Jwt:Secret"]   ?? throw new InvalidOperationException("Jwt:Secret is not configured");
        _issuer   = configuration["Jwt:Issuer"]   ?? "asisya";
        _audience = configuration["Jwt:Audience"] ?? "asisya";
        _expiresHours = int.TryParse(configuration["Jwt:ExpiresHours"], out var h) ? h : 8;
    }

    public string GenerateToken(string username)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_expiresHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
