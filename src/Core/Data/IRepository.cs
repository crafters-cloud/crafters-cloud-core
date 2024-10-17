using CraftersCloud.Core.Entities;

namespace CraftersCloud.Core.Data;

public interface IRepository<T> : IRepository<T, Guid> where T : EntityWithTypedId<Guid>;

public interface IRepository<T, in TId> : IEntityRepository<T> where T : EntityWithTypedId<TId>
{
    void DeleteById(TId id);
    Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);
}