namespace CraftersCloud.Core.Caching.Abstractions;

/// <summary>
/// Represents the configuration options for caching mechanisms, including key management and tagging.
/// </summary>
[PublicAPI]
public record CachingOptions(CacheKey CacheKey)
{
    private IList<string>? _tags;
    public IEnumerable<string>? Tags => _tags?.AsReadOnly();
    
    public static CachingOptions For<T>(string cacheKeySuffix) where T : ICachedQuery =>
        new CachingOptions<T>(cacheKeySuffix);

    public CachingOptions WithTags(params string[] tags)
    {
        _tags ??= [];
        foreach (var tag in tags)
        {
            _tags.Add(tag);
        }

        return this;
    }
}

/// <summary>
/// Represents the configuration settings for managing cache keys, tagging, and cache entry options in caching mechanisms.
/// </summary>
public record CachingOptions<T> : CachingOptions where T : ICachedQuery
{
    internal CachingOptions(string cacheKeySuffix) : base(CacheKey.Create<T>(cacheKeySuffix))
    {
    }
}