using CraftersCloud.Core.AspNetCore.Authorization.Attributes;
using CraftersCloud.Core.AspNetCore.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CraftersCloud.Core.AspNetCore.Authorization;

internal class UserHasPermissionPolicyProvider<TPermission>(IOptions<AuthorizationOptions> options)
    : IAuthorizationPolicyProvider
    where TPermission : notnull
{
    private readonly DefaultAuthorizationPolicyProvider _defaultPolicyProvider = new(options);

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(UserHasPermissionAttribute<TPermission>.PolicyPrefix,
                StringComparison.OrdinalIgnoreCase))
        {
            return _defaultPolicyProvider.GetPolicyAsync(policyName);
        }

        var requirement =
            new UserHasPermissionRequirement<TPermission>(
                PermissionTypeConverter<TPermission>.ConvertFromPolicyName(
                    UserHasPermissionAttribute<TPermission>.PolicyPrefix, policyName));

        return Task.FromResult(new AuthorizationPolicyBuilder().AddRequirements(requirement).Build())!;
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        _defaultPolicyProvider.GetDefaultPolicyAsync(); // DefaultPolicy is RequireAuthenticatedUser

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _defaultPolicyProvider.GetFallbackPolicyAsync();
}