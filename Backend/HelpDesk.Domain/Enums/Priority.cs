namespace HelpDesk.Domain.Enums;

/// <summary>
/// Niveles de prioridad de un ticket
/// </summary>
public enum Priority
{
    /// <summary>
    /// Baja prioridad - puede esperar
    /// </summary>
    Baja = 0,

    /// <summary>
    /// Prioridad media - atender en breve
    /// </summary>
    Media = 1,

    /// <summary>
    /// Alta prioridad - atender inmediatamente
    /// </summary>
    Alta = 2
}
