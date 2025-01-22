namespace CraftersCloud.Core.Entities;

/// <summary>
/// Provides extension methods for querying entities by their IDs.
/// </summary>
[PublicAPI]
public static class QueryableExtensions
{
    /// <summary>
    /// Filters the query to include only entities with the specified ID.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity ID.</typeparam>
    /// <param name="query">The query to filter.</param>
    /// <param name="id">The ID to filter by.</param>
    /// <returns>A queryable collection of entities with the specified ID.</returns>
    public static IQueryable<T> QueryById<T, TId>(this IQueryable<T> query, TId id)
        where T : EntityWithTypedId<TId>
        where TId : struct
        => query.Where(e => e.Id.Equals(id));

    /// <summary>
    /// Filters the query to include only entities with the specified optional ID. If the ID is null the query is not filtered.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity ID.</typeparam>
    /// <param name="query">The query to filter.</param>
    /// <param name="id">The optional ID to filter by.</param>
    /// <returns>A queryable collection of entities with the specified optional ID.</returns>
    public static IQueryable<T> QueryByIdOptional<T, TId>(this IQueryable<T> query, TId? id)
        where T : EntityWithTypedId<TId>
        where TId : struct
        => id == null ? query : query.Where(e => e.Id.Equals(id));

    /// <summary>
    /// Filters the query to exclude entities with the specified ID.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity ID.</typeparam>
    /// <param name="query">The query to filter.</param>
    /// <param name="id">The ID to exclude.</param>
    public static IQueryable<T> QueryExceptWithId<T, TId>(this IQueryable<T> query, TId id)
        where T : EntityWithTypedId<TId>
        where TId : struct
        => query.Where(e => !e.Id.Equals(id));

    /// <summary>
    /// Filters the query to include only entities with the specified optional ID. If the ID is null the query is not filtered.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity ID.</typeparam>
    /// <param name="query">The query to filter.</param>
    /// <param name="id">The optional ID to filter by.</param>
    /// <returns>A queryable collection of entities with the specified optional ID.</returns>
    public static IQueryable<T> QueryExceptWithIdOptional<T, TId>(this IQueryable<T> query, TId? id)
        where T : EntityWithTypedId<TId>
        where TId : struct
        => id == null ? query : query.Where(e => !e.Id.Equals(id));

    /// <summary>
    /// Filters the query to include only entities with the specified IDs.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity ID.</typeparam>
    /// <param name="query">The query to filter.</param>
    /// <param name="ids">The IDs to filter by.</param>
    /// <returns>A queryable collection of entities with the specified IDs.</returns>
    public static IQueryable<T> QueryByIds<T, TId>(this IQueryable<T> query, params TId[] ids)
        where T : EntityWithTypedId<TId>
        where TId : struct =>
        query.Where(e => ids.Contains(e.Id));

    /// <summary>
    /// Filters the query to include only entities with the specified optional IDs. If the IDs are null the query is not filtered.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <typeparam name="TId">The type of the entity ID.</typeparam>
    /// <param name="query">The query to filter.</param>
    /// <param name="ids">The IDs to filter by.</param>
    /// <returns>A queryable collection of entities with the specified IDs.</returns>
    public static IQueryable<T> QueryByIdsOptional<T, TId>(this IQueryable<T> query, params TId[]? ids)
        where T : EntityWithTypedId<TId>
        where TId : struct =>
        ids == null ? query : query.Where(e => ids.Contains(e.Id));
}