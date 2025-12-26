namespace HelpDesk.Domain.Enums;

/// <summary>
/// Estados posibles de un ticket
/// </summary>
public enum TicketStatus
{
    /// <summary>
    /// Ticket recién creado, sin atender
    /// </summary>
    Nuevo = 0,

    /// <summary>
    /// Ticket en proceso de resolución
    /// </summary>
    EnProgreso = 1,

    /// <summary>
    /// Ticket finalizado y resuelto
    /// </summary>
    Resuelto = 2
}
