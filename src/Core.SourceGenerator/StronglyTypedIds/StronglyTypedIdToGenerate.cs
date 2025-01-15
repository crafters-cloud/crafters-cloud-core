using Microsoft.CodeAnalysis;

namespace CraftersCloud.Core.SourceGenerator.StronglyTypedIds;

public readonly record struct StronglyTypedIdToGenerate
{
    public StronglyTypedIdToGenerate(INamedTypeSymbol symbol, AttributeData attribute)
    {
        Symbol = symbol;
        // The first argument of the [StronglyTypedId] attribute is the ValueKind.
        ValueKind = (int) attribute.ConstructorArguments[0].Value!;
    }

    /// <summary>
    /// The symbol of the class to which [StronglyTypedId] attribute is applied.
    /// </summary>
    public INamedTypeSymbol Symbol { get; }

    public string ClassName => Symbol.Name;
    public string Namespace => Symbol.ContainingNamespace.ToDisplayString();
    public int ValueKind { get; }
}