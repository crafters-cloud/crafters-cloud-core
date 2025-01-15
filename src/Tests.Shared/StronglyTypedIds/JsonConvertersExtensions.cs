using System.Reflection;
using Argon;
using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.Tests.Shared.StronglyTypedIds;

[PublicAPI]
public static class JsonConvertersExtensions
{
    public static void AddStronglyTypedIdsJsonConverters(this IList<JsonConverter> converters,
        IEnumerable<Assembly> assemblies)
    {
        var stronglyTypedIds = assemblies.FilterStronglyTypedIds();

        foreach (var stronglyTypedId in stronglyTypedIds)
        {
            var converterType =
                typeof(StronglyTypedIdWriteOnlyJsonConverter<,>).MakeGenericType(stronglyTypedId.Type,
                    stronglyTypedId.ValueType);
            var converter = (JsonConverter) Activator.CreateInstance(converterType)!;
            converters.Add(converter);
        }
    }
    
}