using HelpDesk.Domain.Enums;

namespace HelpDesk.Domain.Entities;

/// <summary>
/// Entidad que representa un ticket de incidencia
/// </summary>
public class Ticket
{
    /// <summary>
    /// Id único del ticket
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Título breve del ticket
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Descripción detallada del problema
    /// </summary>
    public required string Description { get; set; }

    /// <summary>
    /// Prioridad del ticket (Baja, Media, Alta)
    /// </summary>
    public Priority Priority { get; set; } = Priority.Media;

    /// <summary>
    /// Estado actual del ticket (Nuevo, EnProgreso, Resuelto)
    /// </summary>
    public TicketStatus Status { get; set; } = TicketStatus.Nuevo;

    /// <summary>
    /// Fecha de creación del ticket
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de última actualización
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Id del usuario que creó el ticket
    /// </summary>
    public int CreatedByUserId { get; set; }

    /// <summary>
    /// Usuario que creó el ticket
    /// </summary>
    public User? CreatedByUser { get; set; }

    /// <summary>
    /// Id del usuario asignado al ticket (puede ser null)
    /// </summary>
    public int? AssignedToUserId { get; set; }

    /// <summary>
    /// Usuario asignado al ticket
    /// </summary>
    public User? AssignedToUser { get; set; }

    /// <summary>
    /// Notas o comentarios finales del ticket
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Fecha de resolución del ticket (si aplica)
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
}
