using CraftersCloud.Core.AspNetCore.Authorization.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace CraftersCloud.Core.AspNetCore.Authorization.Requirements;

internal class UserHasPermissionRequirement<TPermission>(IEnumerable<TPermission> permissions)
    : IAuthorizationRequirement
    where TPermission : notnull
{
    public IEnumerable<TPermission> Permissions { get; } = permissions;

    public override string ToString() =>
        $"{nameof(UserHasPermissionRequirement<TPermission>)}: " +
        $"{string.Join(",", Permissions.Select(PermissionTypeConverter<TPermission>.ConvertToString))}";
}
