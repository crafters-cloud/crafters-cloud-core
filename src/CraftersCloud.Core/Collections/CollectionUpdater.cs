using System.Collections.ObjectModel;

namespace CraftersCloud.Core.Collections;

public class CollectionUpdater<T, TCollection>
{
    private readonly ICollection<TCollection> _target;
    private readonly bool _applyRemove;

    public CollectionUpdater(ICollection<TCollection> target, bool applyRemove)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _applyRemove = applyRemove;
        Added = new ReadOnlyCollection<(T, TCollection)>(new (T, TCollection)[0]);
        Removed = new ReadOnlyCollection<(T, TCollection)>(new (T, TCollection)[0]);
        Updated = new ReadOnlyCollection<(T, TCollection)>(new (T, TCollection)[0]);
    }

    public IReadOnlyCollection<(T source, TCollection destination)> Added { get; private set; }

    public IReadOnlyCollection<(T source, TCollection destination)> Removed { get; private set; }

    public IReadOnlyCollection<(T source, TCollection destination)> Updated { get; set; }

    public bool HasChanges => Added.Any() || Removed.Any() || Updated.Any();

    public CollectionUpdater<T, TCollection> Apply(ICollection<T> values,
        Func<T, TCollection, bool> matcher,
        Func<T, TCollection> creator,
        Action<TCollection> deleter) =>
        ApplyRemove(values, matcher, deleter)
            .ApplyUpdate(values, matcher, null)
            .ApplyAdd(values, matcher, creator);

    public CollectionUpdater<T, TCollection> Apply(ICollection<T> values,
        Func<T, TCollection, bool> matcher,
        Func<T, TCollection> creator,
        Action<T, TCollection> updater,
        Action<TCollection> deleter) =>
        ApplyRemove(values, matcher, deleter)
            .ApplyUpdate(values, matcher, updater)
            .ApplyAdd(values, matcher, creator);

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

    private CollectionUpdater<T, TCollection> ApplyRemove(ICollection<T> values, Func<T, TCollection, bool> matcher,
        Action<TCollection> deleter)
    {
        var removed = new List<(T, TCollection)>();
        if (_applyRemove)
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