namespace CraftersCloud.Core.Helpers;

public static class TypeExtensions
{
    public static bool ImplementsInterface(this Type concreteType, Type interfaceType) =>
        concreteType.GetInterfaces().Any(t =>
            (interfaceType.IsGenericTypeDefinition && t.IsGenericType
                ? t.GetGenericTypeDefinition()
                : t) == interfaceType);

    public static TAttribute? FindAttribute<TAttribute>(this Type type) where TAttribute : class
    {
        var attributes = type.GetCustomAttributes(typeof(TAttribute), true);
        return attributes.Length > 0
            ? attributes[0] as TAttribute
            : null;
    }
}