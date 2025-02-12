using System.Text;

namespace CraftersCloud.Core.SourceGenerator.StronglyTypedIds.Helpers;

public static class StronglyTypedIdHelper
{
    public static string GenerateClassSource(StronglyTypedIdToGenerate idToGenerate)
    {
        var nameSpace = idToGenerate.Namespace;
        var className = idToGenerate.ClassName;
        var valueKind = idToGenerate.ValueKind;
        var valueType = valueKind switch
        {
            0 => "Guid",
            1 => "Int32",
            _ => throw new InvalidOperationException($"Invalid value valueKind: {valueKind}")
        };
        
        StringBuilder createNewStatement = new($"public static {className} Create({valueType} value) => new {className}(value);");
        
        // If the valueKind is Guid, we need to generate a CreateNew method. Adding CreateNew for Int makes no sense.
        if(valueKind == 0)
        {
            createNewStatement.AppendLine();
            createNewStatement.Append($"  public static {className} CreateNew() => new {className}(SequentialGuidGenerator.Generate());");
        }

        StringBuilder source = new($$"""

                                     // <auto-generated />
                                     #pragma warning disable 1591

                                     using System;
                                     using CraftersCloud.Core.Entities;
                                     using CraftersCloud.Core.StronglyTypedIds;

                                     namespace {{nameSpace}};

                                     readonly partial record struct {{className}}({{valueType}} Value) : IStronglyTypedId<{{valueType}}>
                                     {
                                        public static implicit operator {{valueType}}({{className}} id) => id.Value;
                                     
                                        public override string ToString() => Value.ToString();
                                        
                                        {{createNewStatement}}
                                        
                                        public static bool TryParse(string value, out {{className}} result)
                                        {
                                            if (!{{valueType}}.TryParse(value, out var val))
                                            {
                                                result = default;
                                                return false;
                                            }
                                         
                                             result = new {{className}}(val);
                                             return true;
                                         }
                                     }
                                     """);
        return source.ToString();
    }
}