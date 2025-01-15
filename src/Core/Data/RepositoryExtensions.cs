using CraftersCloud.Core.Entities;
using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.Data;

public static class RepositoryExtensions
{
    public static void AddRange<T>(this IRepository<T> repository, IEnumerable<T> entities) where T : Entity
    {
        foreach (var entity in entities)
        {
            repository.Add(entity);
        }
    }

    public static bool ExistsById<T, TId>(this IRepository<T> repository, TId id)
        where T : EntityWithTypedId<TId>
        where TId : struct =>
        repository.QueryAll().QueryById(id).Any();
}