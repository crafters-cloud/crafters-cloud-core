using Microsoft.CodeAnalysis;

namespace CraftersCloud.Core.SourceGenerator;

public readonly record struct StronglyTypedIdToGenerate(INamedTypeSymbol Symbol, AttributeData Attribute)
{
    public INamedTypeSymbol Symbol { get; } = Symbol;
    public AttributeData Attribute { get; } = Attribute;
}