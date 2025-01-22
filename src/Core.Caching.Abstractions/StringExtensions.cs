
namespace CraftersCloud.Core.Caching.Abstractions;

internal static class StringExtensions
{
    internal static string RemoveCharactersNotSuitableForCacheKey(this string value) => value.Replace('.', ':');
}