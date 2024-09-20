using JetBrains.Annotations;

namespace CraftersCloud.Core.Settings;

[PublicAPI]
public class HealthCheckSettings
{
    public const string HealthChecksSectionName = "HealthChecks";

    public bool Enabled { get; set; }
    public int MaximumAllowedMemoryInMegaBytes { get; set; }
    public bool TokenAuthorizationEnabled { get; set; }
    public string RequiredToken { get; set; } = string.Empty;
}