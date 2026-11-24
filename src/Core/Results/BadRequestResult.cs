namespace CraftersCloud.Core.Results;

/// <summary>
/// The request is invalid (HTTP 400).
/// </summary>
[PublicAPI]
public sealed record BadRequestResult(params IReadOnlyList<ValidationError> Errors);
