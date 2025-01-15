namespace CraftersCloud.Core.Entities;

[PublicAPI]
public static class QueryableExtensions
{
    public static IQueryable<T> QueryById<T, TId>(this IQueryable<T> query, TId id)
        where T : EntityWithTypedId<TId>
        where TId : struct
        => query.Where(e => e.Id.Equals(id));

    public static IQueryable<T> QueryExceptWithId<T, TId>(this IQueryable<T> query, TId id)
        where T : EntityWithTypedId<TId>
        where TId : struct
        => query.Where(e => !e.Id.Equals(id));

    public static IQueryable<T> QueryByIds<T, TId>(this IQueryable<T> query, IEnumerable<TId> ids)
        where T : EntityWithTypedId<TId>
        where TId : struct =>
        query.Where(e => ids.Contains(e.Id));
}