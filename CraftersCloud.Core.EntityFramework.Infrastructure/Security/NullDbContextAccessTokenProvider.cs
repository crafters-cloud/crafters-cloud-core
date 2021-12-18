namespace CraftersCloud.Core.EntityFramework.Infrastructure.Security;

public class NullDbContextAccessTokenProvider : IDbContextAccessTokenProvider
{
    public Task<string> GetAccessTokenAsync() => Task.FromResult(string.Empty);
}