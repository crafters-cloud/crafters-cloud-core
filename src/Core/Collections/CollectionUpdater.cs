using System.Collections.ObjectModel;

namespace CraftersCloud.Core.Collections;

/// <summary>
/// Represents a collection updater that can apply add, update, and remove operations on a target collection.
/// </summary>
/// <typeparam name="T">The type of the source values.</typeparam>
/// <typeparam name="TCollection">The type of the target collection items.</typeparam>
[PublicAPI]
public class CollectionUpdater<T, TCollection>(ICollection<TCollection> target, bool applyRemove)
{
    private readonly ICollection<TCollection> _target = target ?? throw new ArgumentNullException(nameof(target));

    /// <summary>
    /// Gets the collection of added items.
    /// </summary>
    public IReadOnlyCollection<(T source, TCollection destination)> Added { get; private set; } =
        new ReadOnlyCollection<(T, TCollection)>(Array.Empty<(T, TCollection)>());

    /// <summary>
    /// Gets the collection of removed items.
    /// </summary>
    public IReadOnlyCollection<(T source, TCollection destination)> Removed { get; private set; } =
        new ReadOnlyCollection<(T, TCollection)>(Array.Empty<(T, TCollection)>());

    /// <summary>
    /// Gets the collection of updated items.
    /// </summary>
    public IReadOnlyCollection<(T source, TCollection destination)> Updated { get; set; } =
        new ReadOnlyCollection<(T, TCollection)>(Array.Empty<(T, TCollection)>());

    /// <summary>
    /// Gets a value indicating whether there are any changes (added, removed, or updated items).
    /// </summary>
    public bool HasChanges => Added.Count != 0 || Removed.Count != 0 || Updated.Count != 0;

    /// <summary>
    /// Applies the add, update, and remove operations on the target collection.
    /// </summary>
    /// <param name="values">The source values to apply.</param>
    /// <param name="matcher">The function to match source values with target collection items.</param>
    /// <param name="creator">The function to create new target collection items from source values.</param>
    /// <param name="deleter">The action to delete target collection items.</param>
    /// <returns>The current <see cref="CollectionUpdater{T, TCollection}"/> instance.</returns>
    public CollectionUpdater<T, TCollection> Apply(ICollection<T> values,
        Func<T, TCollection, bool> matcher,
        Func<T, TCollection> creator,
        Action<TCollection> deleter) =>
        ApplyRemove(values, matcher, deleter)
            .ApplyUpdate(values, matcher, null)
            .ApplyAdd(values, matcher, creator);

    /// <summary>
    /// Applies the add, update, and remove operations on the target collection.
    /// </summary>
    /// <param name="values">The source values to apply.</param>
    /// <param name="matcher">The function to match source values with target collection items.</param>
    /// <param name="creator">The function to create new target collection items from source values.</param>
    /// <param name="updater">The action to update target collection items with source values.</param>
    /// <param name="deleter">The action to delete target collection items.</param>
    /// <returns>The current <see cref="CollectionUpdater{T, TCollection}"/> instance.</returns>
    public CollectionUpdater<T, TCollection> Apply(ICollection<T> values,
        Func<T, TCollection, bool> matcher,
        Func<T, TCollection> creator,
        Action<T, TCollection> updater,
        Action<TCollection> deleter) =>
        ApplyRemove(values, matcher, deleter)
            .ApplyUpdate(values, matcher, updater)
            .ApplyAdd(values, matcher, creator);

    /// <summary>
    /// Applies the add operation on the target collection.
    /// </summary>
    /// <param name="values">The source values to add.</param>
    /// <param name="matcher">The function to match source values with target collection items.</param>
    /// <param name="creator">The function to create new target collection items from source values.</param>
    /// <returns>The current <see cref="CollectionUpdater{T, TCollection}"/> instance.</returns>
    private CollectionUpdater<T, TCollection> ApplyAdd(ICollection<T> values, Func<T, TCollection, bool> matcher,
        Func<T, TCollection> creator)
    {
        var added = new List<(T, TCollection)>();
        foreach (var item in values)
        {
            if (_target.All(c => !matcher(item, c)))
            {
                var newItem = creator(item);
                _target.Add(newItem);
                added.Add((item, newItem));
            }
        }

        Added = new ReadOnlyCollection<(T, TCollection)>(added);
        return this;
    }

    /// <summary>
    /// Applies the remove operation on the target collection.
    /// </summary>
    /// <param name="values">The source values to match for removal.</param>
    /// <param name="matcher">The function to match source values with target collection items.</param>
    /// <param name="deleter">The action to delete target collection items.</param>
    /// <returns>The current <see cref="CollectionUpdater{T, TCollection}"/> instance.</returns>
    private CollectionUpdater<T, TCollection> ApplyRemove(ICollection<T> values, Func<T, TCollection, bool> matcher,
        Action<TCollection> deleter)
    {
        var removed = new List<(T, TCollection)>();
        if (applyRemove)
        {
            foreach (var original in _target.ToList())
            {
                if (values.All(item => !matcher(item, original)))
                {
                    deleter(original);
                    _target.Remove(original);
                    removed.Add((default!, original));
                }
            }
        }

        Removed = new ReadOnlyCollection<(T, TCollection)>(removed);
        return this;
    }

    /// <summary>
    /// Applies the update operation on the target collection.
    /// </summary>
    /// <param name="values">The source values to update.</param>
    /// <param name="matcher">The function to match source values with target collection items.</param>
    /// <param name="updater">The action to update target collection items with source values.</param>
    /// <returns>The current <see cref="CollectionUpdater{T, TCollection}"/> instance.</returns>
    private CollectionUpdater<T, TCollection> ApplyUpdate(ICollection<T> values, Func<T, TCollection, bool> matcher,
        Action<T, TCollection>? updater)
    {
        if (updater == null)
        {
            return this;
        }

        var updated = new List<(T, TCollection)>();
        foreach (var value in values)
        {
            var match = _target.FirstOrDefault(t => matcher(value, t));
            if (match != null)
            {
                updater(value, match);
                updated.Add((value, match));
            }
        }

        Updated = new ReadOnlyCollection<(T, TCollection)>(updated);
        return this;
    }
}