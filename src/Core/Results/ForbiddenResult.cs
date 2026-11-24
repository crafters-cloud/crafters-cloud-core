namespace CraftersCloud.Core.Results;

/// <summary>
/// The request was valid, but the user is not allowed to perform it (HTTP 403).
/// </summary>
/// <param name="Errors">Optional error messages.</param>
[PublicAPI]
public sealed record ForbiddenResult(params IReadOnlyList<string> Errors);
