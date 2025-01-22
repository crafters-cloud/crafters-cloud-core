namespace CraftersCloud.Core.Caching.Abstractions;

public record CacheKey
{
    private CacheKey(string prefix, string suffix)
    {
        ArgumentException.ThrowIfNullOrEmpty(prefix);
        ArgumentNullException.ThrowIfNull(suffix);
        Prefix = prefix.RemoveCharactersNotSuitableForCacheKey();
        Suffix = suffix.RemoveCharactersNotSuitableForCacheKey();
    }

    public string Prefix { get; init; }
    public string Suffix { get; init; }

    public string FullKey => $"{Prefix}:{Suffix}";
    
    internal static CacheKey Create<T>(string suffix) =>
        Create(typeof(T), suffix);

    private static CacheKey Create(Type type, string suffix)
    {
        var prefix = type.FullName!;
        return new CacheKey(prefix, suffix);
    }
}