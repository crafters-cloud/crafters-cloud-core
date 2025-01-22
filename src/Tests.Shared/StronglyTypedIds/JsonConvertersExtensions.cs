using System.Reflection;
using Argon;
using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.Tests.Shared.StronglyTypedIds;

[PublicAPI]
public static class JsonConvertersExtensions
{
    /// Adds JSON converters for strongly-typed IDs to the provided list of converters.
    /// This method scans the specified assemblies to identify strongly-typed ID implementations
    /// and creates corresponding JSON converters for serialization.
    /// Strongly-typed IDs must implement the `IStronglyTypedId<T>` interface.
    /// <param name="converters">
    /// The list of JSON converters to which the strongly-typed ID converters will be added.
    /// </param>
    /// <param name="assemblies">
    /// A collection of assemblies to search for strongly-typed ID implementations.
    /// </param>
    public static void AddCoreVerifyTestsStronglyTypedIdsJsonConverters(this IList<JsonConverter> converters,
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