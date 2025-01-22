using CraftersCloud.Core.Caching.Abstractions;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace CraftersCloud.Core.MediatR.Caching;

/// <summary>
/// Handles cache eviction notifications by removing cached items associated with specific tags.
/// </summary>
/// <typeparam name="T">The type of notification, which must implement both ICacheEvictor and INotification.</typeparam>
/// <remarks>
/// This class listens to cache eviction notifications and removes cached entries identified by the tags
/// contained within the notification. It uses the HybridCache for performing the cache removal operation.
/// </remarks>
[PublicAPI]
public class CacheEvictorNotificationHandler<T>(HybridCache cache)
    : INotificationHandler<T> where T : ICacheEvictor, INotification
{
    public async Task Handle(T notification, CancellationToken cancellationToken)
    {
        var tasks = notification.Tags.Select(t => cache.RemoveByTagAsync(t, cancellationToken).AsTask());
        await Task.WhenAll(tasks);
    }
}