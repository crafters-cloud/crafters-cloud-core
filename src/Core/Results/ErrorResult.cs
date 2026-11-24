namespace CraftersCloud.Core.Results;

/// <summary>
/// The operation failed for a reason other than invalid input (HTTP 500).
/// </summary>
/// <param name="Errors">Optional error messages.</param>
[PublicAPI]
public sealed record ErrorResult(params IReadOnlyList<string> Errors);
