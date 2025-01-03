using CraftersCloud.Core.Helpers;

namespace CraftersCloud.Core.HealthChecks;

[PublicAPI]
internal class HealthCheckSettings
{
    internal const string SectionName = "HealthChecks";

    public int MaximumAllowedMemoryInMegaBytes { get; set; }
    public string RequiredToken { get; set; } = string.Empty;

    internal bool TokenAuthorizationEnabled => RequiredToken.HasContent();
}