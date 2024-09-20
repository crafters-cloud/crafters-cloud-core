using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace CraftersCloud.Core.HealthChecks.Extensions;

internal static class ConfigurationExtensions
{
    internal static HealthCheckSettings ResolveHealthCheckSettings(this IConfiguration configuration)
        => configuration.GetSection(HealthCheckSettings.SectionName).Get<HealthCheckSettings>() ??
           throw new InvalidOperationException(
               $"Section is missing from configuration. Section Name: {HealthCheckSettings.SectionName}");
}