using System.Text.Json;
using CraftersCloud.Core.Caching.Abstractions;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace CraftersCloud.Core.Caching;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds core caching functionality to the service collection, allowing the configuration of
    /// caching options and enabling hybrid (local and distributed) caching with tailored settings.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the caching services will be added.
    /// </param>
    /// <param name="configureCachingOptions">
    /// A delegate to configure <see cref="CachingOptions"/>, which defines caching behaviors
    /// such as expiration times, Redis settings, and JSON serialization.
    /// </param>
    [PublicAPI]
    public static void AddCoreCaching(this IServiceCollection services, Action<CachingOptions> configureCachingOptions)
    {
        var options = new CachingOptions();
        configureCachingOptions(options);
        
        var config = services.ConfigureCacheConfiguration(options);
        services.AddHybridCache(config, options);
    }
    

    private static CacheSettings ConfigureCacheConfiguration(this IServiceCollection services, CachingOptions options)
    {
        var cacheConfig = new ConfigurationBuilder()
            .SetBasePath(options.CacheSettingsFileBasePath)
            .AddJsonFile(options.CacheSettingsFileName, false, true)
            .Build();

        services.Configure<CacheSettings>(cacheConfig);
        return cacheConfig.Get<CacheSettings>()!;
    }

    private static void AddHybridCache(this IServiceCollection services, CacheSettings config,
        CachingOptions options)
    {
        var fusionCacheBuilder = services.AddFusionCache()
            .WithDefaultEntryOptions(fusionCacheEntryOptions =>
            {
                fusionCacheEntryOptions.Duration = config.DefaultLocalCacheExpiration;
                fusionCacheEntryOptions.DistributedCacheDuration = config.DefaultExpiration;
            })
            .WithSerializer(
                new FusionCacheSystemTextJsonSerializer(ConfigureJsonSerializerOptions(options)));

        if (!string.IsNullOrEmpty(options.RedisConnectionString))
        {
            fusionCacheBuilder.WithDistributedCache(new RedisCache(
                new RedisCacheOptions
                {
                    Configuration = options.RedisConnectionString
                }));
        }

        fusionCacheBuilder.AsHybridCache();
    }

    private static JsonSerializerOptions ConfigureJsonSerializerOptions(CachingOptions cachingOptions)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = false
        };
        cachingOptions.ConfigureJsonSerializerOptions(options);
        return options;
    }
}