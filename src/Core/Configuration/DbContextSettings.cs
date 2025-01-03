namespace CraftersCloud.Core.Configuration;

[PublicAPI]
public class DbContextSettings
{
    public const string SectionName = "DbContext";

    public bool SensitiveDataLoggingEnabled { get; set; }

    public int ConnectionResiliencyMaxRetryCount { get; set; }

    public TimeSpan ConnectionResiliencyMaxRetryDelay { get; set; }

    public bool RegisterMigrationsAssembly { get; set; }
}