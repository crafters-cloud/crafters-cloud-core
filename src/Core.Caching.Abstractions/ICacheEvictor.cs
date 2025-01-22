namespace CraftersCloud.Core.Caching.Abstractions;

/// <summary>
/// Represents an entity capable of defining tags for cache eviction purposes.
/// </summary>
/// <remarks>
/// The interface is designed to provide a mechanism for identifying cached items
/// that should be removed based on associated tags. Implementing classes specify
/// these tags so that the appropriate cache entries can be evicted efficiently.
/// </remarks>
public interface ICacheEvictor
{
    public string[] Tags { get; }
}