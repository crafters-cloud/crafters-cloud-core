using CraftersCloud.Core.HealthChecks.Authorization;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CraftersCloud.Core.HealthChecks.Extensions;

public static class ServiceCollectionExtensions
{
    [PublicAPI]
    public static IHealthChecksBuilder AddCoreHealthChecks(this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetRequiredSection(HealthCheckSettings.SectionName).Get<HealthCheckSettings>()!;
        var healthChecksBuilder = services.AddHealthChecks();

        if (settings.TokenAuthorizationEnabled)
        {
            InitializeTokenAuthorization(services, settings);
        }

        Initialize(healthChecksBuilder, settings);
        return healthChecksBuilder;
    }

    private static void Initialize(IHealthChecksBuilder healthChecksBuilder, HealthCheckSettings healthCheckSettings)
    {
        const int megabyte = 1024 * 1024;
        healthChecksBuilder.AddPrivateMemoryHealthCheck(megabyte * healthCheckSettings.MaximumAllowedMemoryInMegaBytes,
            "Available memory test", HealthStatus.Degraded);
    }

    private static void InitializeTokenAuthorization(IServiceCollection services,
        HealthCheckSettings healthCheckSettings)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(TokenRequirement.Name,
                policy => policy.Requirements.Add(new TokenRequirement(healthCheckSettings.RequiredToken)));
        services.AddSingleton<IAuthorizationHandler, TokenHandler>();
    }
}