using CraftersCloud.Core.Helpers;
using JetBrains.Annotations;

namespace CraftersCloud.Core.HealthChecks;

[PublicAPI]
internal class HealthCheckSettings
{
    internal const string SectionName = "HealthChecks";

    public int MaximumAllowedMemoryInMegaBytes { get; set; }
    public string RequiredToken { get; set; } = string.Empty;

    internal bool TokenAuthorizationEnabled => RequiredToken.HasContent();
}
