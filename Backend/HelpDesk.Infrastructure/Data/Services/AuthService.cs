using HelpDesk.Domain.Entities;
using HelpDesk.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HelpDesk.Infrastructure.Data.Services;

/// <summary>
/// Implementación del servicio de autenticación
/// </summary>
public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    /// <summary>
    /// Constructor del servicio de autenticación
    /// </summary>
    public AuthService(IRepository<User> userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _secretKey = configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        _issuer = configuration["Jwt:Issuer"] ?? "helpdesk-api";
        _audience = configuration["Jwt:Audience"] ?? "helpdesk-client";
    }

    /// <summary>
    /// Autentica un usuario validando credenciales y genera JWT
    /// </summary>
    public async Task<(string Token, User User, DateTime ExpiresAt)?> AuthenticateAsync(string username, string password)
    {
        // Obtener solo el usuario solicitado por nombre de usuario
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Username == username && u.IsActive);

        // Validar credenciales
        if (user == null || user.Password != password)
            return null;

        // Generar token JWT
        var token = GenerateJwtToken(user.Id, user.Username, user.Email);
        var expiresAt = DateTime.UtcNow.AddHours(8);

        return (token, user, expiresAt);
    }

    /// <summary>
    /// Genera un token JWT con los claims del usuario
    /// </summary>
    private string GenerateJwtToken(int userId, string username, string email)
    {
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", userId.ToString()),
                new Claim("username", username),
                new Claim("email", email)
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
