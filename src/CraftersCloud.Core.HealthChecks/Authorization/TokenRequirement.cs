using Microsoft.AspNetCore.Authorization;

namespace CraftersCloud.Core.HealthChecks.Authorization;

internal class TokenRequirement(string token) : IAuthorizationRequirement
{
    public const string Name = "HealthCheckToken";

    public string Token { get; } = token;
}
