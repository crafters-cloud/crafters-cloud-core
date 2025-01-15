using System.Reflection;

namespace CraftersCloud.Core.StronglyTypedIds;

public static class StronglyTypedIdsTypeExtensions
{
    public static IEnumerable<(Type Type, Type ValueType)> FilterStronglyTypedIds(
        this IEnumerable<Assembly> assemblies) =>
        assemblies.SelectMany(a => a.GetTypes())
            .FilterStronglyTypedIds();

    private static IEnumerable<(Type Type, Type ValueType)> FilterStronglyTypedIds(this IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            var stronglyTypedIdInterface = type.GetInterfaces().FirstOrDefault(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStronglyTypedId<>));
            if (stronglyTypedIdInterface != null)
            {
                yield return (type, stronglyTypedIdInterface.GenericTypeArguments[0]);
            }
        }
    }
}