namespace CraftersCloud.Core.Results;

/// <summary>
/// Shortcuts for creating result cases. Each case converts implicitly to any union that contains it
/// (<see cref="QueryResult{T}"/>, <see cref="CreateResult{T}"/>, <see cref="UpdateResult"/>,
/// <see cref="CommandResult{T,TError}"/> or an app-specific union).
/// </summary>
[PublicAPI]
public static class Result
{
    /// <summary>
    /// A new resource was created (HTTP 201).
    /// </summary>
    public static CreatedResult<T> Created<T>(T value) => new(value);

    /// <summary>
    /// The operation succeeded and there is nothing to return (HTTP 204).
    /// </summary>
    public static NoContentResult NoContent() => new();

    /// <summary>
    /// The requested resource was not found (HTTP 404).
    /// </summary>
    public static NotFoundResult NotFound(params IReadOnlyList<string> errorMessages) => new(errorMessages);

    /// <summary>
    /// The request was valid, but the user is not allowed to perform it (HTTP 403).
    /// </summary>
    public static ForbiddenResult Forbidden(params IReadOnlyList<string> errorMessages) => new(errorMessages);

    /// <summary>
    /// The operation failed for a reason other than invalid input (HTTP 500).
    /// </summary>
    public static ErrorResult Error(params IReadOnlyList<string> errorMessages) => new(errorMessages);

    /// <summary>
    /// The request is invalid (HTTP 400).
    /// </summary>
    public static BadRequestResult BadRequest(params IReadOnlyList<ValidationError> errors) => new(errors);

    /// <summary>
    /// The request is invalid (HTTP 400) because of a single property.
    /// </summary>
    public static BadRequestResult BadRequest(string propertyName, string message, string? code = null) =>
        new([new ValidationError(propertyName, message, code)]);

    /// <summary>
    /// Whether <paramref name="value"/> is one of the failure cases (<see cref="NotFoundResult"/>,
    /// <see cref="ForbiddenResult"/>, <see cref="ErrorResult"/>, <see cref="BadRequestResult"/>).
    /// </summary>
    public static bool IsFailure(object? value) =>
        value is NotFoundResult or ForbiddenResult or ErrorResult or BadRequestResult;
}
