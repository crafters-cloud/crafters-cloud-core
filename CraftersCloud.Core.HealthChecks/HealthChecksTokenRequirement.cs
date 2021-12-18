﻿using Microsoft.AspNetCore.Authorization;

namespace CraftersCloud.Core.HealthChecks;

internal class HealthChecksTokenRequirement : IAuthorizationRequirement
{
    public const string Name = "HealthCheckToken";

    public HealthChecksTokenRequirement(string token) => Token = token;

    public string Token { get; }
}