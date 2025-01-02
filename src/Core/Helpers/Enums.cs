using JetBrains.Annotations;

namespace CraftersCloud.Core.Helpers;

[PublicAPI]
public static class Enums
{
    public static T ValueFrom<T>(string value) where T : Enum => (T) Enum.Parse(typeof(T), value);

    public static IEnumerable<T> GetAll<T>() =>
        Enum.GetValues(typeof(T))
            .Cast<T>();
}