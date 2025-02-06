using System.Reflection;
using System.Text.Json.Serialization;
using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.SystemTextJson;

[PublicAPI]
public static class JsonConverterExtensions
{
    /// <summary>
    /// Register SmartEnum json converters for System.Text.Json
    /// </summary>
    /// <param name="converters">List of converters to add converters to</param>
    /// <param name="assembliesWithStronglyTypedIds">Assemblies containing SmartEnums</param>
    public static void AddCoreStronglyTypedIdsJsonConverters(this IList<JsonConverter> converters,
        IEnumerable<Assembly> assembliesWithStronglyTypedIds)
    {
        var stronglyTypedIds = assembliesWithStronglyTypedIds.FilterStronglyTypedIds();

        foreach (var values in stronglyTypedIds)
        {
            var converterType =
                typeof(StronglyTypedIdJsonConverter<,>).MakeGenericType(values.Type, values.ValueType);
            var converter = (JsonConverter) Activator.CreateInstance(converterType)!;
            converters.Add(converter);
        }
    }
}