using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.Entities;

public static class QueryableExtensions
{
    public static IQueryable<T> QueryById<T>(this IQueryable<T> query, IStronglyTypedId<Guid> id) where T : EntityWithTypedId<IStronglyTypedId<Guid>> => 
        query.Where(e => e.Id == id);
    
    public static IQueryable<T> QueryById<T>(this IQueryable<T> query, IStronglyTypedId<int> id) where T : EntityWithTypedId<IStronglyTypedId<int>> => 
        query.Where(e => e.Id == id);

    public static IQueryable<T> QueryById<T>(this IQueryable<T> query, Guid id) where T : EntityWithTypedId<Guid> =>
        query.Where(e => e.Id == id);

    public static IQueryable<T> QueryById<T>(this IQueryable<T> query, int id) where T : EntityWithTypedId<int> =>
        query.Where(e => e.Id == id);
    
    public static IQueryable<T> QueryExceptWithId<T>(this IQueryable<T> query, IStronglyTypedId<Guid> id) where T : EntityWithTypedId<IStronglyTypedId<Guid>> => 
        query.Where(e => e.Id != id);
    
    public static IQueryable<T> QueryExceptWithId<T>(this IQueryable<T> query, IStronglyTypedId<int> id) where T : EntityWithTypedId<IStronglyTypedId<int>> => 
        query.Where(e => e.Id != id);
    
    public static IQueryable<T> QueryExceptWithId<T>(this IQueryable<T> query, Guid? id)
        where T : EntityWithTypedId<Guid> =>
        query.Where(e => e.Id != id);

    public static IQueryable<T> QueryExceptWithId<T>(this IQueryable<T> query, int? id)
        where T : EntityWithTypedId<int> =>
        query.Where(e => e.Id != id);
    
    public static IQueryable<T> QueryByIds<T>(this IQueryable<T> query, IEnumerable<IStronglyTypedId<Guid>> ids)
        where T : EntityWithTypedId<IStronglyTypedId<Guid>> =>
        query.Where(e => ids.Contains(e.Id));
    
    public static IQueryable<T> QueryByIds<T>(this IQueryable<T> query, IEnumerable<IStronglyTypedId<int>> ids)
        where T : EntityWithTypedId<IStronglyTypedId<int>> =>
        query.Where(e => ids.Contains(e.Id));

    public static IQueryable<T> QueryByIds<T>(this IQueryable<T> query, IEnumerable<Guid> ids)
        where T : EntityWithTypedId<Guid> =>
        query.Where(e => ids.Contains(e.Id));

    public static IQueryable<T> QueryByIds<T>(this IQueryable<T> query, IEnumerable<int> ids)
        where T : EntityWithTypedId<int> =>
        query.Where(e => ids.Contains(e.Id));
}