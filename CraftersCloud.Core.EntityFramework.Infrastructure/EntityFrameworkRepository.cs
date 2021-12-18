using System.Linq.Expressions;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[UsedImplicitly]
public class EntityFrameworkRepository<T> : IRepository<T> where T : Entity
{
    public EntityFrameworkRepository(DbContext context)
    {
        DbContext = context;
        DbSet = context.Set<T>();
    }

    protected DbSet<T> DbSet { get; }

    protected DbContext DbContext { get; }

    public virtual IQueryable<T> QueryAll() => DbSet;

    public virtual IQueryable<T> QueryAllSkipCache() => DbSet.AsNoTracking();

    public virtual IQueryable<T> QueryAllIncluding(params Expression<Func<T, object>>[] paths) =>
        paths.Aggregate(QueryAll(),
            (current, includeProperty) => current.Include(includeProperty));

    public virtual IQueryable<T> QueryAllSkipCacheIncluding(params Expression<Func<T, object>>[] paths) =>
        paths.Aggregate(QueryAllSkipCache(),
            (current, includeProperty) => current.Include(includeProperty));


    public virtual void Add(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        DbSet.Add(entity);
    }

    public virtual async Task AddAsync(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        await DbSet.AddAsync(item);
    }

    public virtual void Delete(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        DbSet.Remove(item);
    }

    public virtual void DeleteRange(IEnumerable<T> entities) => DbSet.RemoveRange(entities);
}

[UsedImplicitly]
public class EntityFrameworkRepository<T, TId> : EntityFrameworkRepository<T>, IRepository<T, TId>
    where T : EntityWithTypedId<TId>
{
    public EntityFrameworkRepository(DbContext context) : base(context)
    {
    }

    public virtual void Delete(TId id)
    {
        var item = FindById(id);
        if (item != null)
        {
            Delete(item);
        }
    }

    public virtual T? FindById(TId id) => DbSet.Find(id);

    public virtual async Task<T?> FindByIdAsync(TId id) => await DbSet.FindAsync(id);
}