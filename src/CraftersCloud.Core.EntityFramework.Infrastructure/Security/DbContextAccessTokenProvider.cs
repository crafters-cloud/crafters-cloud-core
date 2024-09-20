using Azure.Core;
using Azure.Identity;
using CraftersCloud.Core.Helpers;
using CraftersCloud.Core.Settings;
using Microsoft.Extensions.Logging;

namespace CraftersCloud.Core.EntityFramework.Infrastructure.Security;

public class DbContextAccessTokenProvider(DbContextSettings settings, ILogger<DbContextAccessTokenProvider> logger)
    : IDbContextAccessTokenProvider
{
    public async Task<string> GetAccessTokenAsync() =>
        settings.UseAccessToken
            ? await GetTokenFromAzureServiceTokenProvider()
            : await Task.FromResult(string.Empty);

    private async Task<string> GetTokenFromAzureServiceTokenProvider()
    {
        try
        {
            var tokenCredential = new DefaultAzureCredential();
            var accessToken = await tokenCredential.GetTokenAsync(
                new TokenRequestContext(["https://database.windows.net/.default"]));

            if (!accessToken.Token.HasContent())
            {
                logger.LogWarning("Getting access token for managed service identity: Token is empty.");
            }

            return accessToken.Token;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error when getting access token for managed service identity");
            throw;
        }
    }
}