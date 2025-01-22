using CraftersCloud.Core.Caching.Abstractions;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CraftersCloud.Core.MediatR.Caching;

/// <summary>
/// Represents a MediatR pipeline behavior that integrates caching capabilities for query handling.
/// </summary>
/// <typeparam name="TRequest">
/// The type of the request object, which must implement the <see cref="ICachedQuery"/> interface.
/// </typeparam>
/// <typeparam name="TResponse">
/// The type of the response object returned by the request's handler.
/// </typeparam>
/// <remarks>
/// This class is responsible for handling the caching behavior during the MediatR pipeline execution.
/// It retrieves a cached response if available; otherwise, executes the next pipeline delegate to get the response
/// and stores the result in the cache for future use.
/// </remarks>
/// <example>
/// Implements the caching mechanism using <see cref="HybridCache"/> to ensure performant and consistent data access
/// with specific caching options provided by the request.
/// </example>
/// <threadsafety>
/// This class is thread-safe when used in a concurrent environment.
/// </threadsafety>
/// <seealso cref="ICachedQuery"/>
/// <seealso cref="IPipelineBehavior{TRequest,TResponse}"/>
[PublicAPI]
public class CachingPipelineBehavior<TRequest, TResponse>(
    HybridCache cache,
    ILogger<CachingPipelineBehavior<TRequest, TResponse>> logger,
    IOptions<CacheSettings> cacheSettingsOptions,
    ILogger<CacheSettings> cacheSettingsLogger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cachingOptions = request.CachingOptions;
        var (cacheKey, setting) = cacheSettingsOptions.Value.GetCacheKeyAndSetting(cachingOptions.CacheKey, cacheSettingsLogger);
        var tags = cachingOptions.Tags;

        return await cache.GetOrCreateAsync<TResponse>(cacheKey.FullKey, async _ =>
        {
            logger.LogDebug("Object not found in cache by {Key}. Continuing the pipeline execution to get the object",
                cacheKey.FullKey);
            return await next();
        }, new HybridCacheEntryOptions
        {
            LocalCacheExpiration = setting.LocalCacheExpiration,
            Expiration = setting.Expiration
        }, cancellationToken: cancellationToken, tags: tags);
    }
}