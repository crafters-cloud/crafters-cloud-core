using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
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

    public virtual void Add(T entity) => DbSet.Add(entity);

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

    public virtual void Delete(T item) => DbSet.Remove(item);

    public virtual void DeleteRange(IEnumerable<T> entities) => DbSet.RemoveRange(entities);

    private T? FindById(TId id) => DbSet.Find(id);

    private ValueTask<T?> FindByIdAsync(TId id, CancellationToken cancellationToken) =>
        DbSet.FindAsync([id], cancellationToken);
}