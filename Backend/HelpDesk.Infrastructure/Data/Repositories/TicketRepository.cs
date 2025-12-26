using HelpDesk.Domain.Entities;
using HelpDesk.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure.Data.Repositories;

/// <summary>
/// Repositorio especializado para Tickets
/// Incluye métodos específicos de filtrado y búsqueda
/// </summary>
public interface ITicketRepository : IRepository<Ticket>
{
    /// <summary>
    /// Obtiene tickets con paginación, filtrado por estado y prioridad
    /// </summary>
    Task<(IEnumerable<Ticket> Items, int Total)> GetTicketsPagedAsync(
        int page = 1,
        int pageSize = 10,
        TicketStatus? status = null,
        Priority? priority = null);

    /// <summary>
    /// Obtiene tickets asignados a un usuario específico
    /// </summary>
    Task<IEnumerable<Ticket>> GetTicketsByAssignedUserAsync(int userId);

    /// <summary>
    /// Obtiene tickets creados por un usuario específico
    /// </summary>
    Task<IEnumerable<Ticket>> GetTicketsByCreatedUserAsync(int userId);

    /// <summary>
    /// Busca tickets por título o descripción
    /// </summary>
    Task<IEnumerable<Ticket>> SearchTicketsAsync(string searchTerm);
}

/// <summary>
/// Implementación del repositorio de Tickets
/// </summary>
public class TicketRepository : Repository<Ticket>, ITicketRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor del repositorio de tickets
    /// </summary>
    public TicketRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene un ticket por ID con sus relaciones cargadas
    /// </summary>
    public override async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Obtiene tickets con paginación y filtros
    /// </summary>
    public async Task<(IEnumerable<Ticket> Items, int Total)> GetTicketsPagedAsync(
        int page = 1,
        int pageSize = 10,
        TicketStatus? status = null,
        Priority? priority = null)
    {
        var query = _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .AsQueryable();

        // Filtrar por estado si se proporciona
        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        // Filtrar por prioridad si se proporciona
        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        // Contar total de registros
        int total = await query.CountAsync();

        // Aplicar paginación
        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    /// <summary>
    /// Obtiene tickets asignados a un usuario
    /// </summary>
    public async Task<IEnumerable<Ticket>> GetTicketsByAssignedUserAsync(int userId)
    {
        return await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .Where(t => t.AssignedToUserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene tickets creados por un usuario
    /// </summary>
    public async Task<IEnumerable<Ticket>> GetTicketsByCreatedUserAsync(int userId)
    {
        return await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .Where(t => t.CreatedByUserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Busca tickets por título o descripción
    /// </summary>
    public async Task<IEnumerable<Ticket>> SearchTicketsAsync(string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return await _context.Tickets
            .Include(t => t.CreatedByUser)
            .Include(t => t.AssignedToUser)
            .Where(t => t.Title.ToLower().Contains(lowerSearchTerm) ||
                        t.Description.ToLower().Contains(lowerSearchTerm))
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}
