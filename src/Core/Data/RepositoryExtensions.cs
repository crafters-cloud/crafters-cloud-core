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
    
    public static bool QueryExists<T>(this IRepository<T> repository, IStronglyTypedId<Guid> id)
        where T : EntityWithTypedId<IStronglyTypedId<Guid>> =>
        repository.QueryAll().QueryById(id).Any();
    
    public static bool QueryExists<T>(this IRepository<T> repository, IStronglyTypedId<int> id)
        where T : EntityWithTypedId<IStronglyTypedId<int>> =>
        repository.QueryAll().QueryById(id).Any();

    public static bool QueryExists<T>(this IRepository<T> repository, Guid id)
        where T : EntityWithTypedId<Guid> =>
        repository.QueryAll().QueryById(id).Any();
    
    public static bool QueryExists<T>(this IRepository<T> repository, int id)
        where T : EntityWithTypedId<int> =>
        repository.QueryAll().QueryById(id).Any();
}