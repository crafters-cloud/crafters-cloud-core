namespace CraftersCloud.Core.Results;

/// <summary>
/// The requested resource was not found (HTTP 404).
/// </summary>
/// <param name="Errors">Optional error messages.</param>
[PublicAPI]
public sealed record NotFoundResult(params IReadOnlyList<string> Errors);
