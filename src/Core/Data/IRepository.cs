using CraftersCloud.Core.Entities;

namespace CraftersCloud.Core.Data;

public interface IRepository<T> where T : Entity
{
    /// <summary>
    /// Returns all entities of type T
    /// </summary>
    /// <returns>Non executed query</returns>
    IQueryable<T> QueryAll();

    /// <summary>
    /// Adds a new entity of type T
    /// </summary>
    /// <param name="entity">The entity to add</param>
    void Add(T entity);

    /// <summary>
    /// Deletes an existing entity of type T
    /// </summary>
    /// <param name="entity">The entity to delete</param>
    void Delete(T entity);

    /// <summary>
    /// Deletes a range of entities of type T
    /// </summary>
    /// <param name="entities">The entities to delete</param>
    void DeleteRange(IEnumerable<T> entities);
}

public interface IRepository<T, in TId> : IRepository<T> where T : EntityWithTypedId<TId>
{
    void DeleteById(TId id);
    Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);
}

// public interface IRepository<T> : IRepository<T, Guid> where T : EntityWithTypedId<Guid>;