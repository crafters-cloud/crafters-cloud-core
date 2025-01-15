using System.Collections.Immutable;
using System.Text;
using CraftersCloud.Core.SourceGenerator.StronglyTypedIds.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CraftersCloud.Core.SourceGenerator.StronglyTypedIds;

[Generator]
public class StronglyTypedIdGenerator : IIncrementalGenerator
{
    private const string AttributeName = StronglyTypedIdAttributeHelper.AttributeName;
    private const string AttributeNamespace = StronglyTypedIdAttributeHelper.AttributeNamespace;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource($"{StronglyTypedIdAttributeHelper.ValueKindEnumName}.g.cs",
                StronglyTypedIdAttributeHelper.ValueKindEnumText);
            ctx.AddSource($"{StronglyTypedIdAttributeHelper.AttributeName}.g.cs",
                StronglyTypedIdAttributeHelper.AttributeText);
        });

        var stronglyTypedIdClasses = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                $"{AttributeNamespace}.{AttributeName}",
                static (s, _) => IsSyntaxTargetForGeneration(s),
                static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null)
            .Collect();

        context.RegisterSourceOutput(stronglyTypedIdClasses, Execute);
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
        node is RecordDeclarationSyntax recordDeclarationSyntax
        && recordDeclarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword);

    private static StronglyTypedIdToGenerate? GetSemanticTargetForGeneration(GeneratorAttributeSyntaxContext context)
    {
        var symbol = context.TargetSymbol;

        if (symbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return null;
        }

        var stronglyTypedAttributeData = namedTypeSymbol.GetAttributes().FirstOrDefault(IsStronglyTypedAttribute);
        
        return stronglyTypedAttributeData is null ? null : new StronglyTypedIdToGenerate(namedTypeSymbol, stronglyTypedAttributeData);
    }

    private static bool IsStronglyTypedAttribute(AttributeData ad) =>
        string.Equals(ad.AttributeClass?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            $"global::{AttributeNamespace}.{AttributeName}");

    private static void Execute(SourceProductionContext context, ImmutableArray<StronglyTypedIdToGenerate?> idsToGenerate)
    {
        foreach (var id in idsToGenerate.Where(idToGenerate => idToGenerate is not null))
        {
            var classSource = ProcessClass(id!.Value, context);

            if (classSource is null)
            {
                continue;
            }

            var symbol = id.Value.Symbol;

            context.AddSource($"{symbol.ContainingNamespace}_{symbol.Name}.g.cs", classSource);
        }
    }

    private static string? ProcessClass(StronglyTypedIdToGenerate idToGenerate, SourceProductionContext context)
    {
        var classSymbol = idToGenerate.Symbol;
        var attributeLocation = classSymbol.Locations.FirstOrDefault() ?? Location.None;

        if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
        {
            CreateDiagnosticError(GeneratorDiagnosticDescriptors.TopLevelError);
            return null;
        }
        
        return StronglyTypedIdHelper.GenerateClassSource(idToGenerate);

        void CreateDiagnosticError(DiagnosticDescriptor descriptor)
        {
            context.ReportDiagnostic(Diagnostic.Create(descriptor, attributeLocation, classSymbol.Name,
                DiagnosticSeverity.Error));
        }
    }
}