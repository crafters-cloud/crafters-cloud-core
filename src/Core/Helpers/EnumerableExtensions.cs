namespace CraftersCloud.Core.Helpers;

[PublicAPI]
public static class EnumerableExtensions
{
    public static IEnumerable<T> ToEmptyIfNull<T>(this IEnumerable<T>? source) => source ?? [];
}