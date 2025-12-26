namespace HelpDesk.Infrastructure.Data.Repositories;

/// <summary>
/// Interfaz genérica para repositorios
/// Define operaciones CRUD básicas
/// </summary>
/// <typeparam name="TEntity">Tipo de entidad</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Obtiene todas las entidades
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Obtiene una entidad por id
    /// </summary>
    Task<TEntity?> GetByIdAsync(int id);

    /// <summary>
    /// Agrega una nueva entidad
    /// </summary>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Actualiza una entidad existente
    /// </summary>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Elimina una entidad
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// Elimina una entidad específica
    /// </summary>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Guarda los cambios en la base de datos
    /// </summary>
    Task SaveChangesAsync();

    /// <summary>
    /// Verifica si existe una entidad con el id especificado
    /// </summary>
    Task<bool> ExistsAsync(int id);
}
