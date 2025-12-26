using HelpDesk.Domain.Entities;

namespace HelpDesk.Infrastructure.Data.Services;

/// <summary>
/// Servicio de autenticación
/// Maneja la autenticación de usuarios y generación de tokens JWT
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Autentica un usuario y genera un token JWT
    /// </summary>
    /// <param name="username">Nombre de usuario</param>
    /// <param name="password">Contraseña</param>
    /// <returns>Token JWT y datos del usuario, o null si falla la autenticación</returns>
    Task<(string Token, User User, DateTime ExpiresAt)?> AuthenticateAsync(string username, string password);
}
