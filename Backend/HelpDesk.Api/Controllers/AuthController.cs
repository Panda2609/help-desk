using FluentValidation;
using HelpDesk.Api.Models;
using HelpDesk.Infrastructure.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, IValidator<LoginRequest> loginValidator, ILogger<AuthController> logger)
    {
        _authService = authService;
        _loginValidator = loginValidator;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Validate request
        var validationResult = await _loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(new 
            { 
                message = "Validation failed",
                errors = validationResult.Errors.Select(e => new { field = e.PropertyName, error = e.ErrorMessage })
            });

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
