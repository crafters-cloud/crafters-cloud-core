using CraftersCloud.Core.Entities;

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

    public static bool EntityExists<T>(this IRepository<T> repository, Guid id)
        where T : EntityWithTypedId<Guid> =>
        repository.QueryAll().EntityExists(id);

    public static bool EntityExists<T>(this IRepository<T> repository, int id)
        where T : EntityWithTypedId<int> =>
        repository.QueryAll().EntityExists(id);
}