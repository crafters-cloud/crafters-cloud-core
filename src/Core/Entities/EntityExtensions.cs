namespace CraftersCloud.Core.Entities;

public static class EntityExtensions
{
    public static T WithId<T, TId>(this T entity, TId id) where T : EntityWithTypedId<TId>
    {
        entity.Id = id;
        return entity;
    }
}