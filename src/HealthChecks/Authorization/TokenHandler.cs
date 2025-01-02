using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CraftersCloud.Core.HealthChecks.Authorization;

internal class TokenHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<TokenRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenRequirement requirement)
    {
        if (httpContextAccessor.HttpContext?.Request.Query["token"].ToString() == requirement.Token)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}