using Microsoft.CodeAnalysis;

namespace CraftersCloud.Core.SourceGenerator.StronglyTypedIds.Helpers
{
    public static class GeneratorDiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor TopLevelError = new(id: "CCCOREGEN001",
                                                                                              title: "Class must be top level",
                                                                                              messageFormat: "Class '{0}' using StronglyTypedId must be top level",
                                                                                              category: "StronglyTypedId",
                                                                                              DiagnosticSeverity.Error,
                                                                                              isEnabledByDefault: true);

        public static readonly DiagnosticDescriptor WrongType = new(id: "CCCOREGEN002",
                                                                                              title: "Class must be record struct",
                                                                                              messageFormat: "Class '{0}' must be record struct",
                                                                                              category: "StronglyTypedId",
                                                                                              DiagnosticSeverity.Error,
                                                                                              isEnabledByDefault: true);
    }
}
