namespace CraftersCloud.Core.Collections;

/// <summary>
/// Provides extension methods for updating collections.
/// </summary>
public static class CollectionUpdaterExtensions
{
    /// <summary>
    /// Updates the collection with the specified values, using the provided matcher, creator, updater, and optional deleter.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <typeparam name="TCollection">The type of the collection items.</typeparam>
    /// <param name="collection">The collection to update.</param>
    /// <param name="values">The values to update the collection with.</param>
    /// <param name="matcher">The function to match values with collection items.</param>
    /// <param name="creator">The function to create new collection items from values.</param>
    /// <param name="updater">The action to update collection items with values.</param>
    /// <param name="deleter">The optional action to invoke when delete collection items.</param>
    /// <returns>A <see cref="CollectionUpdater{T, TCollection}"/> instance.</returns>
    public static CollectionUpdater<T, TCollection> UpdateWith<T, TCollection>(
        this ICollection<TCollection> collection,
        ICollection<T> values,
        Func<T, TCollection, bool> matcher,
        Func<T, TCollection> creator,
        Action<T, TCollection> updater,
        Action<TCollection>? deleter = null) =>
        new CollectionUpdater<T, TCollection>(collection, true).Apply(values, matcher, creator, updater,
            deleter ?? NullDeleter<TCollection>());

    /// <summary>
    /// Updates the collection with the specified values, using the provided matcher, creator, and updater, without removing any items.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <typeparam name="TCollection">The type of the collection items.</typeparam>
    /// <param name="collection">The collection to update.</param>
    /// <param name="values">The values to update the collection with.</param>
    /// <param name="matcher">The function to match values with collection items.</param>
    /// <param name="creator">The function to create new collection items from values.</param>
    /// <param name="updater">The action to update collection items with values.</param>
    /// <returns>A <see cref="CollectionUpdater{T, TCollection}"/> instance.</returns>
    public static CollectionUpdater<T, TCollection> UpdateWithoutRemove<T, TCollection>(
        this ICollection<TCollection> collection,
        ICollection<T> values,
        Func<T, TCollection, bool> matcher,
        Func<T, TCollection> creator,
        Action<T, TCollection> updater) =>
        new CollectionUpdater<T, TCollection>(collection, false).Apply(values, matcher, creator, updater,
            NullDeleter<TCollection>());

    /// <summary>
    /// Provides a no-op deleter action.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection items.</typeparam>
    /// <returns>An action that does nothing.</returns>
    private static Action<TCollection> NullDeleter<TCollection>() => item => { };

    /// <summary>
    /// Updates the collection with the specified values, using the provided matcher and creator.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <typeparam name="TCollection">The type of the collection items.</typeparam>
    /// <param name="collection">The collection to update.</param>
    /// <param name="values">The values to update the collection with.</param>
    /// <param name="matcher">The function to match values with collection items.</param>
    /// <param name="creator">The function to create new collection items from values.</param>
    /// <returns>A <see cref="CollectionUpdater{T, TCollection}"/> instance.</returns>
    public static CollectionUpdater<T, TCollection> UpdateWith<T, TCollection>(
        this ICollection<TCollection> collection,
        ICollection<T> values,
        Func<T, TCollection, bool> matcher,
        Func<T, TCollection> creator) =>
        new CollectionUpdater<T, TCollection>(collection, true).Apply(values, matcher, creator,
            NullDeleter<TCollection>());

    /// <summary>
    /// Adds or updates an item in the collection based on the specified matcher, creator, and updater.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TCollection">The type of the collection items.</typeparam>
    /// <param name="collection">The collection to add or update the item in.</param>
    /// <param name="value">The value to add or update.</param>
    /// <param name="matcher">The function to match the value with a collection item.</param>
    /// <param name="creator">The function to create a new collection item from the value.</param>
    /// <param name="updater">The action to update the collection item with the value.</param>
    /// <returns>The added or updated collection item.</returns>
    public static TCollection AddOrUpdate<T, TCollection>(this ICollection<TCollection> collection, T value,
        Func<TCollection, bool> matcher, Func<T, TCollection> creator, Action<T, TCollection> updater)
    {
        var entity = collection.SingleOrDefault(matcher);

        if (entity == null)
        {
            entity = creator(value);
            collection.Add(entity);
        }
        else
        {
            updater(value, entity);
        }

        return entity;
    }
}