using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.AspNetCore.Authorization.Requirements;

internal class UserHasPermissionRequirementHandler<TPermission>(
    IAuthorizationProvider<TPermission> authorizationProvider,
    ILogger<AuthenticatedUserRequirementHandler<UserHasPermissionRequirement<TPermission>>> logger)
    : AuthenticatedUserRequirementHandler<UserHasPermissionRequirement<TPermission>>(logger)
    where TPermission : notnull
{
    protected override bool FulfillsRequirement(AuthorizationHandlerContext context,
        UserHasPermissionRequirement<TPermission> requirement) =>
        authorizationProvider.AuthorizePermissions(requirement.Permissions);
}