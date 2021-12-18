namespace CraftersCloud.Core.EntityFramework.Infrastructure.Security;

public interface IDbContextAccessTokenProvider
{
    Task<string> GetAccessTokenAsync();
}