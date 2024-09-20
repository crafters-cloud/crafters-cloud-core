using System.Linq.Expressions;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[UsedImplicitly]
public class EntityFrameworkRepository<T>(DbContext context)
    : EntityFrameworkRepository<T, Guid>(context), IRepository<T>
    where T : EntityWithTypedId<Guid>;

[UsedImplicitly]
public class EntityFrameworkRepository<T, TId>(DbContext context) : IRepository<T, TId>
    where T : EntityWithTypedId<TId>
{
    protected DbSet<T> DbSet { get; } = context.Set<T>();

    protected DbContext DbContext { get; } = context;

    public virtual IQueryable<T> QueryAll() => DbSet;

    public virtual IQueryable<T> QueryAllSkipCache() => DbSet.AsNoTracking();

    public virtual IQueryable<T> QueryAllIncluding(params Expression<Func<T, object>>[] paths) =>
        paths.Aggregate(QueryAll(),
            (current, includeProperty) => current.Include(includeProperty));

    public virtual IQueryable<T> QueryAllSkipCacheIncluding(params Expression<Func<T, object>>[] paths) =>
        paths.Aggregate(QueryAllSkipCache(),
            (current, includeProperty) => current.Include(includeProperty));


    public virtual void Add(T entity) => DbSet.Add(entity);

    public virtual void Delete(TId id)
    {
        var item = FindById(id);
        if (item != null)
        {
            Delete(item);
        }
    }

    public virtual void Delete(T item) => DbSet.Remove(item);

    public virtual void DeleteRange(IEnumerable<T> entities) => DbSet.RemoveRange(entities);

    public virtual T? FindById(TId id) => DbSet.Find(id);

    public virtual async Task<T?> FindByIdAsync(TId id) => await DbSet.FindAsync(id);
}