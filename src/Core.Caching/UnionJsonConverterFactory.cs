using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CraftersCloud.Core.Caching;

/// <summary>
/// Serializes C# unions with the case they hold, so they can be read back from a distributed cache.
/// </summary>
/// <remarks>
/// System.Text.Json writes a union as the bare value of its case, which can't be read back when several cases
/// are JSON objects (e.g. <c>QueryResult&lt;UserDto&gt;</c> holding a <c>UserDto</c> or a <c>NotFoundResult</c>).
/// This converter writes <c>{"case": "&lt;case type&gt;", "value": ...}</c> instead.
/// A value whose case is unknown (e.g. written by an older version of the union) fails to deserialize,
/// which the cache treats as a miss.
/// </remarks>
[PublicAPI]
public sealed class UnionJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) =>
        typeToConvert.IsValueType && typeof(IUnion).IsAssignableFrom(typeToConvert);

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) =>
        (JsonConverter) Activator.CreateInstance(typeof(UnionJsonConverter<>).MakeGenericType(typeToConvert))!;

    private sealed class UnionJsonConverter<TUnion> : JsonConverter<TUnion> where TUnion : struct, IUnion
    {
        private const string CasePropertyName = "case";
        private const string ValuePropertyName = "value";

        private static readonly (Type CaseType, string Name, ConstructorInfo Constructor)[] Cases =
        [
            .. typeof(TUnion).GetConstructors()
                .Select(c => (Constructor: c, Parameters: c.GetParameters()))
                .Where(c => c.Parameters.Length == 1)
                .Select(c => (c.Parameters[0].ParameterType, CaseName(c.Parameters[0].ParameterType), c.Constructor))
        ];

        public override void Write(Utf8JsonWriter writer, TUnion value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            var caseValue = value.Value;
            if (caseValue is null)
            {
                writer.WriteNull(CasePropertyName);
            }
            else
            {
                var caseType = FindCase(caseValue);
                writer.WriteString(CasePropertyName, caseType.Name);
                writer.WritePropertyName(ValuePropertyName);
                JsonSerializer.Serialize(writer, caseValue, caseType.CaseType, options);
            }

            writer.WriteEndObject();
        }

        public override TUnion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Expect(reader.TokenType == JsonTokenType.StartObject);
            ReadPropertyName(ref reader, CasePropertyName);
            reader.Read();
            if (reader.TokenType == JsonTokenType.Null)
            {
                reader.Read();
                Expect(reader.TokenType == JsonTokenType.EndObject);
                return default;
            }

            var name = reader.GetString();
            var unionCase = Array.Find(Cases, c => c.Name == name);
            if (unionCase.Constructor is null)
            {
                throw new JsonException($"Unknown case '{name}' for union {typeof(TUnion)}.");
            }

            ReadPropertyName(ref reader, ValuePropertyName);
            reader.Read();
            var caseValue = JsonSerializer.Deserialize(ref reader, unionCase.CaseType, options);
            reader.Read();
            Expect(reader.TokenType == JsonTokenType.EndObject);
            return (TUnion) unionCase.Constructor.Invoke([caseValue]);
        }

        private static (Type CaseType, string Name, ConstructorInfo Constructor) FindCase(object caseValue)
        {
            var runtimeType = caseValue.GetType();
            var exact = Array.Find(Cases, c => c.CaseType == runtimeType);
            return exact.Constructor is not null
                ? exact
                : Array.Find(Cases, c => c.CaseType.IsInstanceOfType(caseValue));
        }

        private static void ReadPropertyName(ref Utf8JsonReader reader, string propertyName)
        {
            reader.Read();
            Expect(reader.TokenType == JsonTokenType.PropertyName && reader.ValueTextEquals(propertyName));
        }

        private static void Expect(bool condition)
        {
            if (!condition)
            {
                throw new JsonException($"Invalid JSON for union {typeof(TUnion)}.");
            }
        }

        private static string CaseName(Type caseType) => caseType.ToString();
    }
}
