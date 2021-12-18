namespace CraftersCloud.Core.AspNetCore.Tests.Http;

public static class UriExtensions
{
    public static Uri AppendParameters(this Uri uri, params (string key, string value)[] parameters)
    {
        var resourceUri = uri.ToString();

        var paramsUri = string.Join("&",
            parameters.Select(p => Uri.EscapeDataString(p.key) + "=" + Uri.EscapeDataString(p.value)));

        if (!string.IsNullOrEmpty(paramsUri))
        {
            resourceUri += "?" + paramsUri;
        }

        return new Uri(resourceUri, UriKind.Relative);
    }
}