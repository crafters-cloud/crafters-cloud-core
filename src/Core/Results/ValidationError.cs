namespace CraftersCloud.Core.Results;

/// <summary>
/// A single validation error. Independent of any validation library.
/// </summary>
/// <param name="PropertyName">The (camel cased) name of the invalid property; empty for errors that apply to the whole request.</param>
/// <param name="Message">A human-readable message.</param>
/// <param name="Code">An optional machine-readable code (e.g. FluentValidation's <c>ErrorCode</c>).</param>
[PublicAPI]
public sealed record ValidationError(string PropertyName, string Message, string? Code = null);
