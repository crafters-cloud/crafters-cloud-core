namespace CraftersCloud.Core.Caching.Abstractions;

/// <summary>
/// Represents configuration options for an individual cache entry.
/// </summary>
/// <remarks>
/// This class defines properties that configure the key and expiration durations
/// for a specific cache entry. It can be used to customize cache behavior at the
/// entry level.
/// </remarks>
/// <example>
/// The <see cref="CacheEntrySetting"/> can be utilized to specific expiration
/// and cache behavior for individual items in a caching mechanism.
/// </example>
/// <remarks>
/// - <c>Key</c>: Specifies the identifier for the cache entry.
/// - <c>Expiration</c>: Configures the duration the cache entry is valid in a distributed cache.
/// - <c>LocalCacheExpiration</c>: Configures the duration the cache entry is valid in a local cache.
/// </remarks>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public sealed class CacheEntrySetting
{
    public string Key { get; init; } = string.Empty;
    public TimeSpan Expiration { get; init; }
    public TimeSpan LocalCacheExpiration { get; init; }
}