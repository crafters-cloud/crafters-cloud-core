using CraftersCloud.Core.Results.Types;

namespace CraftersCloud.Core.Results;

public static class Result
{
    /// <summary>
    /// Represents a successful operation and accepts a values as the result of the operation
    /// </summary>
    /// <param name="value">Sets the Value property</param>
    /// <returns>A Result<typeparamref name="T"/></returns>
    public static SuccessResult<T> Success<T>(T value) => new(value, string.Empty);

    /// <summary>
    /// Represents a successful operation and accepts a values as the result of the operation
    /// Sets the SuccessMessage property to the provided value
    /// </summary>
    /// <param name="value">Sets the Value property</param>
    /// <param name="successMessage">Sets the SuccessMessage property</param>
    /// <returns>A Result<typeparamref name="T"/></returns>
    public static SuccessResult<T> Success<T>(T value, string successMessage) => new(value, successMessage);

    /// <summary>
    /// Represents a successful operation that resulted in the creation of a new resource.
    /// </summary>
    /// <typeparam name="T">The type of the resource created.</typeparam>
    /// <returns>A Result<typeparamref name="T"/> with status Created.</returns>
    public static CreatedResult<T> Created<T>(T value) => new(value);

    /// <summary>
    /// Represents the situation where a service was unable to find a requested resource.
    /// </summary>
    /// <returns>A Result<typeparamref name="T"/></returns>
    public static NotFoundResult NotFound() => new();

    /// <summary>
    /// Represents the situation where a service was unable to find a requested resource.
    /// Error messages may be provided and will be exposed via the Errors property.
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param>
    /// <returns>A Result<typeparamref name="T"/></returns>
    public static NotFoundResult NotFound(params string[] errorMessages) => new() { Errors = errorMessages };

    /// <summary>
    /// The parameters to the call were correct, but the user does not have permission to perform some action.
    /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <returns>A Result<typeparamref name="T"/></returns>
    public static ForbiddenResult Forbidden() => new();

    /// <summary>
    /// The parameters to the call were correct, but the user does not have permission to perform some action.
    /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param> 
    /// <returns>A Result<typeparamref name="T"/></returns>
    public static ForbiddenResult Forbidden(params string[] errorMessages) => new() { Errors = errorMessages };

    /// <summary>
    /// Represents a critical error that occurred during the execution of the service.
    /// Everything provided by the user was valid, but the service was unable to complete due to an exception.
    /// See also HTTP 500 Internal Server Error: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
    /// </summary>
    /// <param name="errorMessages">A list of string error messages.</param>
    /// <returns>A Result<typeparamref name="T"/></returns>
    public static ErrorResult Error(params string[] errorMessages) => new() { Errors = errorMessages };

    /// <summary>
    /// Represents a situation where the server has successfully fulfilled the request, but there is no content to send back in the response body.
    /// </summary>
    /// <typeparam name="T">The type parameter representing the expected response data.</typeparam>
    /// <returns>A Result object</returns>
    public static NoContentResult NoContent() => new();
}