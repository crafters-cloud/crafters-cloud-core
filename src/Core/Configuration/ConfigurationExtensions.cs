using Microsoft.Extensions.Configuration;

namespace CraftersCloud.Core.Configuration;

public static class ConfigurationExtensions
{
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName)
    {
        var sectionSettings = configuration.GetSection(sectionName).Get<T>();
        return sectionSettings == null
            ? throw new InvalidOperationException($"Section is missing from configuration. Section Name: {sectionName}")
            : sectionSettings;
    }
}