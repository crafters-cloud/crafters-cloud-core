using CraftersCloud.Core.StronglyTypedIds;
using NJsonSchema;
using NJsonSchema.Generation;

namespace CraftersCloud.Core.Swagger;

/// <summary>
/// A schema processor for handling strongly-typed ID types within OpenAPI/Swagger schema generation.
/// </summary>
/// <remarks>
/// This processor identifies types implementing <see cref="IStronglyTypedId{TValue}"/> and adjusts their
/// JSON schema representation accordingly. It maps the ID's underlying value type (e.g., int, Guid, string)
/// to the appropriate OpenAPI type and format.
/// </remarks>
/// <example>
/// For example, an <see cref="IStronglyTypedId{TValue}"/> of type Guid will be represented as a string with the "uuid" format.
/// </example>
/// <seealso cref="ISchemaProcessor"/>
internal class StronglyTypeIdSwaggerSchemaProcessor : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        var contextualType = context.ContextualType;
        var schema = context.Schema;
        var type = contextualType.OriginalType;

        var stronglyTypedMetaData = type.GetStronglyTypedIdMetaData();

        if (stronglyTypedMetaData == null)
        {
            // not a strongly typed id
            return;
        }
        

        // Clear default schema details
        schema.Items.Clear();
        schema.AllOf.Clear();

        if (stronglyTypedMetaData.ValueType == typeof(int))
        {
            schema.Type = JsonObjectType.Integer;
            schema.Format = "int32";
        }
        else if (stronglyTypedMetaData.ValueType == typeof(Guid))
        {
            schema.Type = JsonObjectType.String;
            schema.Format = "uuid"; // Assuming strongly-typed IDs are GUIDs or similar
        }
        else
        {
            schema.Type = JsonObjectType.String;
            schema.Format = "string";
        }
    }
}