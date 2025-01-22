using System.Reflection;

namespace CraftersCloud.Core.StronglyTypedIds;

internal static class TypeExtensions
{
    internal static IEnumerable<StronglyTypedIdMetaData> FilterStronglyTypedIds(
        this IEnumerable<Assembly> assemblies) =>
        assemblies.SelectMany(a => a.GetTypes())
            .FilterStronglyTypedIds();

    internal static IEnumerable<StronglyTypedIdMetaData> FilterStronglyTypedIds(this IEnumerable<Type> types) =>
        types.Select(type => type.GetStronglyTypedIdMetaData())
            .Where(metaData => metaData != null)
            .Select(metaData => metaData!);

    internal static StronglyTypedIdMetaData? GetStronglyTypedIdMetaData(this Type type)
    {
        var stronglyTypedIdInterface = type.GetInterfaces().FirstOrDefault(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStronglyTypedId<>));
        return stronglyTypedIdInterface != null
            ? new StronglyTypedIdMetaData(type, stronglyTypedIdInterface.GenericTypeArguments[0])
            : null;
    }
}