using HelpDesk.Api.Models;
using HelpDesk.Infrastructure.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.AuthenticateAsync(request.Username, request.Password);

        if (result == null)
            return Unauthorized(new { message = "Credenciales inválidas" });

        var (token, user, expiresAt) = result.Value;
        var response = new LoginResponse
        {
            Token = token,
            User = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email
            },
            ExpiresAt = expiresAt
        };

        return Ok(response);
    }
}
