﻿using System.Reflection;
using System.Text.Json.Serialization;
using Ardalis.SmartEnum.SystemTextJson;

namespace CraftersCloud.Core.SmartEnums.SystemTextJson;

[PublicAPI]
public static class JsonConverterExtensions
{
    /// <summary>
    /// Register SmartEnum json converters for System.Text.Json
    /// </summary>
    /// <param name="converters">List of converters to add converters to</param>
    /// <param name="assembliesWithSmartEnums">Assemblies containing SmartEnums</param>
    public static void AddCoreSmartEnumJsonConverters(this IList<JsonConverter> converters,
        IEnumerable<Assembly> assembliesWithSmartEnums)
    {
        var smartEnums = assembliesWithSmartEnums.FindSmartEnums();

        foreach (var smartEnumsType in smartEnums)
        {
            var converterType =
                typeof(SmartEnumValueConverter<,>).MakeGenericType(smartEnumsType.EnumType, smartEnumsType.ValueType);
            var converter = (JsonConverter) Activator.CreateInstance(converterType)!;
            converters.Add(converter);
        }
    }
}