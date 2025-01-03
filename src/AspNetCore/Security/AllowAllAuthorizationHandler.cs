using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.AspNetCore.Security;

[UsedImplicitly]
public class AllowAllAuthorizationHandler(ILogger<AllowAllAuthorizationHandler> logger) : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        logger.LogWarning("Allowing all requests. This message should not appear in production!!");
        var pendingRequirements = context.PendingRequirements.ToList();
        foreach (var requirement in pendingRequirements)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}