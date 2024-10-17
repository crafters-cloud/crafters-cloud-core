using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.AspNetCore.Security;

[UsedImplicitly]
public class AllowAllAuthorizationHandler : IAuthorizationHandler
{
    private readonly ILogger<AllowAllAuthorizationHandler> _logger;

    public AllowAllAuthorizationHandler(ILogger<AllowAllAuthorizationHandler> logger) => _logger = logger;

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        _logger.LogWarning("Allowing all requests. This message should not appear in production!!");
        var pendingRequirements = context.PendingRequirements.ToList();
        foreach (var requirement in pendingRequirements)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}