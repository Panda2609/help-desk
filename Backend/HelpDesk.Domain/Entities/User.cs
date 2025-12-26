namespace HelpDesk.Domain.Entities;

/// <summary>
/// Entidad que representa un usuario del sistema
/// </summary>
public class User
{
    /// <summary>
    /// Id único del usuario
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre de usuario para autenticación
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Contraseña del usuario (en producción debe estar hasheada)
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Nombre completo del usuario
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Email del usuario
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Fecha de creación del usuario
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indica si el usuario está activo
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Colección de tickets asignados a este usuario
    /// </summary>
    public ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
}
