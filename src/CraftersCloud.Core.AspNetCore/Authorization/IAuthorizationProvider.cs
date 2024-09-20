namespace CraftersCloud.Core.AspNetCore.Authorization;

public interface IAuthorizationProvider<in TPermission> where TPermission : notnull
{
    public bool AuthorizePermissions(IEnumerable<TPermission> permissions);
}
