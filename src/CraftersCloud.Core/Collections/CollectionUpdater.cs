using System.Collections.ObjectModel;
using JetBrains.Annotations;

namespace CraftersCloud.Core.Collections
{
    [PublicAPI]
    public class CollectionUpdater<T, TCollection>(ICollection<TCollection> target, bool applyRemove)
    {
        private readonly ICollection<TCollection> _target = target ?? throw new ArgumentNullException(nameof(target));

        public IReadOnlyCollection<(T source, TCollection destination)> Added { get; private set; } = new ReadOnlyCollection<(T, TCollection)>(Array.Empty<(T, TCollection)>());

        public IReadOnlyCollection<(T source, TCollection destination)> Removed { get; private set; } = new ReadOnlyCollection<(T, TCollection)>(Array.Empty<(T, TCollection)>());

        public IReadOnlyCollection<(T source, TCollection destination)> Updated { get; set; } = new ReadOnlyCollection<(T, TCollection)>(Array.Empty<(T, TCollection)>());

        public bool HasChanges => Added.Count != 0 || Removed.Count != 0 || Updated.Count != 0;

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
            foreach (T item in values)
            {
                if (_target.All(c => !matcher(item, c)))
                {
                    TCollection newItem = creator(item);
                    _target.Add(newItem);
                    added.Add((item, newItem));
                }
            }

            Added = new ReadOnlyCollection<(T, TCollection)>(added);
            return this;
        }

        private CollectionUpdater<T, TCollection> ApplyRemove(ICollection<T> values, Func<T, TCollection, bool> matcher, Action<TCollection> deleter)
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
                        removed.Add((default(T)!, original));
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
            foreach (T value in values)
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
}
