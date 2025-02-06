using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.SystemTextJson;

public class StronglyTypedIdJsonConverter<T, TValue> : JsonConverter<T>
    where T : class, IStronglyTypedId<TValue> where TValue : struct
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.Null ? null : GetFromValue(ReadValue(ref reader));

    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();
        }
        else if (typeof(TValue) == typeof(int))
        {
            writer.WriteNumberValue((int) (object) value.Value);
        }
        else
        {
            writer.WriteStringValue(value.Value.ToString());
        }
    }

    private static TValue ReadValue(ref Utf8JsonReader reader)
    {
        if (typeof(TValue) == typeof(int))
        {
            return (TValue) (ValueType) reader.GetInt32();
        }

        if (typeof(TValue) == typeof(Guid))
        {
            return (TValue) (ValueType) reader.GetGuid();
        }

        throw new ArgumentOutOfRangeException(typeof(TValue).ToString(), typeof(TValue).Name + " is not supported.");
    }

    private static T GetFromValue(TValue value)
    {
        try
        {
            var createMethod = typeof(T).GetMethod("Create", BindingFlags.Public | BindingFlags.Static, null,
                [typeof(TValue)], null)!;
            return (T) createMethod.Invoke(null, [value])!;
        }
        catch (Exception ex)
        {
            throw new JsonException($"Error converting value '{value}' to a strongly typed id.", ex);
        }
    }
}