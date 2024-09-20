using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace CraftersCloud.Core.HealthChecks.Authorization;

internal class TokenHandler(IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<TokenRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenRequirement requirement)
    {
        if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request.Query["token"].ToString() == requirement.Token)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
