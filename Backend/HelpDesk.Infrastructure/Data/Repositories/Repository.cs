using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Infrastructure.Data.Repositories;

/// <summary>
/// Implementación genérica del repositorio
/// Proporciona operaciones CRUD básicas para cualquier entidad
/// </summary>
/// <typeparam name="TEntity">Tipo de entidad</typeparam>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    /// <summary>
    /// Constructor del repositorio
    /// </summary>
    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Obtiene una entidad por id
    /// </summary>
    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Agrega una nueva entidad
    /// </summary>
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);
        await SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Elimina una entidad por id
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }
    }

    /// <summary>
    /// Elimina una entidad específica
    /// </summary>
    public async Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);
        await SaveChangesAsync();
    }

    /// <summary>
    /// Guarda los cambios en la base de datos
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Verifica si existe una entidad con el id especificado
    /// </summary>
    public async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.FindAsync(id) != null;
    }
}
