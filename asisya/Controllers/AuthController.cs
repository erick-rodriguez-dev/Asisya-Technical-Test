using asisya.Application.DTOs.Auth;
using asisya.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace asisya.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _service.LoginAsync(dto);
        return token is null ? Unauthorized("Credenciales inválidas.") : Ok(token);
    }
}
