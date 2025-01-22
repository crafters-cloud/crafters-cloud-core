using CraftersCloud.Core.Cqrs;

namespace CraftersCloud.Core.Caching.Abstractions;

/// <summary>
/// Defines a query interface with caching capabilities, extending the base query structure.
/// </summary>
[PublicAPI]
public interface ICachedQuery : IBaseQuery
{
    CachingOptions CachingOptions { get; }
}

/// <summary>
/// Represents a query interface that supports caching functionality by incorporating caching options.
/// </summary>
[PublicAPI]
public interface ICachedQuery<out TResponse> : ICachedQuery, IQuery<TResponse>;