using System.Runtime.CompilerServices;
using CraftersCloud.Core.Caching.Abstractions;
using CraftersCloud.Core.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ZiggyCreatures.Caching.Fusion;

namespace CraftersCloud.Core.MediatR.Caching;

/// <summary>
/// A pipeline behavior that caches the responses of queries implementing <see cref="ICachedQuery"/>.
/// </summary>
/// <typeparam name="TRequest">
/// The type of the request object, which must implement the <see cref="ICachedQuery"/> interface.
/// </typeparam>
/// <typeparam name="TResponse">The type of the response object returned by the request handler.</typeparam>
/// <remarks>
/// The cache key and tags come from <see cref="ICachedQuery.CachingOptions"/>, the expirations from
/// <see cref="CacheSettings"/>. When the response is a union holding a failure case (e.g. <see cref="NotFoundResult"/>
/// in a <see cref="QueryResult{T}"/>), it is cached only if that case is configured in
/// <see cref="CacheSettings.DefaultFailureCases"/> or <see cref="CacheEntrySetting.FailureCases"/>, with its own expiration.
/// </remarks>
/// <seealso cref="ICachedQuery"/>
/// <seealso cref="CacheSettings"/>
[PublicAPI]
public class CachingPipelineBehavior<TRequest, TResponse>(
    IFusionCache cache,
    ILogger<CachingPipelineBehavior<TRequest, TResponse>> logger,
    IOptions<CacheSettings> cacheSettingsOptions,
    ILogger<CacheSettings> cacheSettingsLogger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var cacheSettings = cacheSettingsOptions.Value;
        var cachingOptions = request.CachingOptions;
        var (cacheKey, setting) = cacheSettings.GetCacheKeyAndSetting(cachingOptions.CacheKey, cacheSettingsLogger);

        var entryOptions = cache.CreateEntryOptions(options =>
        {
            options.Duration = setting.LocalCacheExpiration;
            options.DistributedCacheDuration = setting.Expiration;
        });

        return await cache.GetOrSetAsync<TResponse>(cacheKey.FullKey, async (context, _) =>
        {
            logger.LogDebug("Object not found in cache by {Key}. Continuing the pipeline execution to get the object",
                cacheKey.FullKey);
            var response = await next();

            if (response is IUnion union && Result.IsFailure(union.Value))
            {
                var failureCase = union.Value!.GetType().Name;
                var expiration = cacheSettings.GetFailureCaseExpiration(setting, failureCase);
                if (expiration is { } failureExpiration)
                {
                    logger.LogDebug("Caching {FailureCase} for {Key} for {Expiration}", failureCase, cacheKey.FullKey,
                        failureExpiration);
                    context.Options.Duration = Min(failureExpiration, setting.LocalCacheExpiration);
                    context.Options.DistributedCacheDuration = failureExpiration;
                }
                else
                {
                    logger.LogDebug("Not caching {FailureCase} for {Key}", failureCase, cacheKey.FullKey);
                    context.Options.SkipMemoryCacheWrite = true;
                    context.Options.SkipDistributedCacheWrite = true;
                }
            }

            return response;
        }, options: entryOptions, tags: cachingOptions.Tags, token: cancellationToken);
    }

    private static TimeSpan Min(TimeSpan a, TimeSpan b) => a < b ? a : b;
}
