using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[UsedImplicitly]
public class EntityFrameworkRepository<T>(DbContext context) : IRepository<T>
    where T : Entity
{
    protected DbSet<T> DbSet { get; } = context.Set<T>();

    protected DbContext DbContext { get; } = context;

    public virtual IQueryable<T> QueryAll() => DbSet;

    public virtual void Add(T entity) => DbSet.Add(entity);

    public virtual void Delete(T entity) => DbSet.Remove(entity);

    public virtual void DeleteRange(IEnumerable<T> entities) => DbSet.RemoveRange(entities);
}

[UsedImplicitly]
public class EntityFrameworkRepository<T, TId>(DbContext context)
    : EntityFrameworkRepository<T>(context), IRepository<T, TId>
    where T : EntityWithTypedId<TId>
{
    public virtual void DeleteById(TId id)
    {
        var item = FindById(id);
        if (item != null)
        {
            Delete(item);
        }
    }

    public async Task DeleteByIdAsync(TId id, CancellationToken cancellationToken)
    {
        var item = await FindByIdAsync(id, cancellationToken);
        if (item != null)
        {
            Delete(item);
        }
    }

    private T? FindById(TId id) => DbSet.Find(id);

    private ValueTask<T?> FindByIdAsync(TId id, CancellationToken cancellationToken) =>
        DbSet.FindAsync([id], cancellationToken);
}