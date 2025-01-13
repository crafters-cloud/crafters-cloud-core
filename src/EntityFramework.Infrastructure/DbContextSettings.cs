using System.ComponentModel.DataAnnotations;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[PublicAPI]
public class DbContextSettings
{
    public const string SectionName = "DbContext";

    public bool SensitiveDataLoggingEnabled { get; init; }

    [Range(0, 100)]
    public int ConnectionResiliencyMaxRetryCount { get; init; }

    public TimeSpan ConnectionResiliencyMaxRetryDelay { get; init; }

    public bool RegisterMigrationsAssembly { get; init; }
}