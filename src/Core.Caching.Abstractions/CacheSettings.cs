using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.Caching.Abstractions;

/// <summary>
/// Represents configuration options for cache entries used by the caching system.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class CacheSettings
{
    private Dictionary<string, CacheEntrySetting> _entrySettings = [];

    /// <summary>
    /// Specifies a prefix to be removed from the beginning of the cache key, if present.
    /// </summary>
    public string? CacheKeyPrefixToRemove { get; set; }

    /// <summary>
    /// Defines the default duration for items to remain in the local cache before they expire.
    /// </summary>
    public TimeSpan DefaultLocalCacheExpiration { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Specifies the default expiration duration for cache entries.
    /// This is the time span after which a cache entry is considered expired if no specific expiration setting is provided.
    /// </summary>
    public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Defines an array of cache entry settings, each representing configuration options
    /// for individual cache entries, including keys and expiration durations.
    /// </summary>
    public CacheEntrySetting[] Entries
    {
        get => [.. _entrySettings.Values];
        init => _entrySettings = value.ToDictionary(x => x.Key, x => x);
    }

    /// Retrieves the cache entry setting associated with a specific cache key.
    /// If a corresponding setting is not found, a default value is returned.
    /// <param name="key">The unique cache key used to look up the cache entry setting.</param>
    /// <param name="logger">The logger</param>
    /// <returns>The <see cref="CacheEntrySetting"/> associated with the specified key, or a default setting if the key is not found.</returns>
    public (CacheKey key, CacheEntrySetting setting) GetCacheKeyAndSetting(CacheKey key, ILogger<CacheSettings> logger)
    {
        key = RemovePrefix(key);

        var lookupKey = key.Prefix;
        if (_entrySettings.TryGetValue(lookupKey, out var setting))
        {
            logger.LogDebug("Cache entry setting found: {LookupKey}, {CacheKey}, {LocalCacheExpiration}, {Expiration}", lookupKey, key.FullKey,
                setting.LocalCacheExpiration, setting.Expiration);
            return (key, setting);
        }

        logger.LogWarning(
            "Cache entry setting not found for {LookupKey}. Using the default value. If caching is not needed remove the marker interface from the request.",
            lookupKey);
        return (key, DefaultCacheEntrySetting(lookupKey));
    }

    // by removing the prefix we reduce the size of the cache key and remove the boilerplate (e.g. repeating root namespaces, etc.)
    private CacheKey RemovePrefix(CacheKey key)
    {
        if (CacheKeyPrefixToRemove == null)
        {
            return key;
        }

        var prefix = key.Prefix.Replace(CacheKeyPrefixToRemove, string.Empty);
        return key with { Prefix = prefix };
    }

    private CacheEntrySetting DefaultCacheEntrySetting(string key) =>
        new() { Key = key, LocalCacheExpiration = DefaultLocalCacheExpiration, Expiration = DefaultExpiration };
}