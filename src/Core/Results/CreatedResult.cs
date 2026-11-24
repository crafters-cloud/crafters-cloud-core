namespace CraftersCloud.Core.Results;

/// <summary>
/// A new resource was created (HTTP 201).
/// </summary>
[PublicAPI]
public sealed record CreatedResult<T>(T Value);
