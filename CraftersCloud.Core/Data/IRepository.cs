using System.Linq.Expressions;
using CraftersCloud.Core.Entities;

namespace CraftersCloud.Core.Data;

public interface IRepository<T> where T : Entity
{
    void Add(T entity);

    Task AddAsync(T entity);

    void Delete(T entity);

    void DeleteRange(IEnumerable<T> entities);

    IQueryable<T> QueryAll();

    IQueryable<T> QueryAllSkipCache();

    IQueryable<T> QueryAllIncluding(params Expression<Func<T, object>>[] paths);

    IQueryable<T> QueryAllSkipCacheIncluding(params Expression<Func<T, object>>[] paths);
}

public interface IRepository<T, in TId> : IRepository<T> where T : EntityWithTypedId<TId>
{
    void Delete(TId id);

    T? FindById(TId id);

    Task<T?> FindByIdAsync(TId id);
}