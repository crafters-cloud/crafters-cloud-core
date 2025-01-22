using System.Text.Json;

namespace CraftersCloud.Core.Caching;

/// <summary>
/// Provides configuration options for caching, including cache expiration, Redis connection settings,
/// and JSON serialization configuration.
/// </summary>
[PublicAPI]
public class CachingOptions
{
    /// <summary>
    /// Specifies the connection string used to configure the connection to a Redis instance.
    /// This is utilized for setting up a distributed cache backed by Redis.
    /// If the RedisConnectionString is not supplied, cache will not use Redis
    /// </summary>
    public string? RedisConnectionString { get; set; }

    /// <summary>
    /// Specifies a delegate to configure JSON serialization options used for caching operations.
    /// This property allows customization of the <see cref="System.Text.Json.JsonSerializerOptions"/>
    /// instance to fine-tune the behavior of JSON serialization and deserialization,
    /// such as naming policies, case sensitivity, and other settings.
    /// </summary>
    public Action<JsonSerializerOptions> ConfigureJsonSerializerOptions { get; set; } = _ => { };

    /// <summary>
    /// Specifies the base directory path used for locating the cache settings configuration file.
    /// This property defines the directory where the cache settings file is expected to reside,
    /// enabling the caching system to load configuration parameters from the specified path.
    /// </summary>
    public string CacheSettingsFileBasePath { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the name of the JSON file containing cache settings.
    /// This property determines the file used to configure cache entry options,
    /// including defaults and custom settings for caching behavior.
    /// </summary>
    public string CacheSettingsFileName { get; set; } = "cacheSettings.json";
}