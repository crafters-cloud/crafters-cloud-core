﻿using NSwag.Generation.AspNetCore;

namespace CraftersCloud.Core.SmartEnums.Swagger;

[PublicAPI]
public static class AspNetCoreOpenApiDocumentGeneratorSettingsExtensions
{
    /// <summary>
    /// Configure SmartEnums for NSwag. Adds SmartEnumSwaggerSchemaProcessor to SchemaProcessors
    /// </summary>
    /// <param name="settings">Settings to configure</param>
    public static void CoreConfigureSmartEnums(this AspNetCoreOpenApiDocumentGeneratorSettings settings) => 
        settings.SchemaSettings.SchemaProcessors.Add(new SmartEnumSwaggerSchemaProcessor());
}