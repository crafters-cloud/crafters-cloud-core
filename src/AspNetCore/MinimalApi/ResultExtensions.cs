using CraftersCloud.Core.AspNetCore.Errors;
using CraftersCloud.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ValidationProblemDetails = Microsoft.AspNetCore.Mvc.ValidationProblemDetails;

namespace CraftersCloud.Core.AspNetCore.MinimalApi;

/// <summary>
/// Maps the standard result unions to Minimal API typed results, so OpenAPI sees every status code.
/// </summary>
/// <remarks>
/// Don't return a union from an endpoint directly: ASP.NET Core serializes the case it holds with 200 OK,
/// even for a failure. For <see cref="CommandResult{T,TError}"/>, switch over the result in the endpoint and use
/// <see cref="ToMinimalApiResult(BadRequestResult, HttpContext)"/> for the <see cref="BadRequestResult"/> case.
/// </remarks>
[PublicAPI]
public static class ResultExtensions
{
    /// <summary>
    /// 200 OK with the mapped item, or 404 Not Found.
    /// </summary>
    public static Results<Ok<TDestination>, NotFound> ToMinimalApiResult<TSource, TDestination>(
        this QueryResult<TSource> result, Func<TSource, TDestination> mapper) =>
        result switch
        {
            TSource value => TypedResults.Ok(mapper(value)),
            NotFoundResult => TypedResults.NotFound(),
            null => throw Uninitialized()
        };

    /// <summary>
    /// 200 OK with the item, or 404 Not Found.
    /// </summary>
    public static Results<Ok<T>, NotFound> ToMinimalApiResult<T>(this QueryResult<T> result) =>
        result.ToMinimalApiResult(x => x);

    /// <summary>
    /// 201 Created with the mapped value at the location returned by <paramref name="location"/>,
    /// or 400 Bad Request with validation problem details.
    /// </summary>
    public static Results<Created<TDestination>, BadRequest<ValidationProblemDetails>> ToMinimalApiResult<T, TDestination>(
        this CreateResult<T> result, HttpContext httpContext, Func<T, string> location, Func<T, TDestination> mapper) =>
        result switch
        {
            CreatedResult<T> created => TypedResults.Created(location(created.Value), mapper(created.Value)),
            BadRequestResult badRequest => badRequest.ToMinimalApiResult(httpContext),
            null => throw Uninitialized()
        };

    /// <summary>
    /// 201 Created with the value at the location returned by <paramref name="location"/>,
    /// or 400 Bad Request with validation problem details.
    /// </summary>
    public static Results<Created<T>, BadRequest<ValidationProblemDetails>> ToMinimalApiResult<T>(
        this CreateResult<T> result, HttpContext httpContext, Func<T, string> location) =>
        result.ToMinimalApiResult(httpContext, location, x => x);

    /// <summary>
    /// 204 No Content, 404 Not Found, or 400 Bad Request with validation problem details.
    /// </summary>
    public static Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>> ToMinimalApiResult(
        this UpdateResult result, HttpContext httpContext) =>
        result switch
        {
            NoContentResult => TypedResults.NoContent(),
            NotFoundResult => TypedResults.NotFound(),
            BadRequestResult badRequest => badRequest.ToMinimalApiResult(httpContext),
            null => throw Uninitialized()
        };

    /// <summary>
    /// 400 Bad Request with validation problem details.
    /// </summary>
    public static BadRequest<ValidationProblemDetails> ToMinimalApiResult(this BadRequestResult result,
        HttpContext httpContext) =>
        TypedResults.BadRequest(httpContext.CreateValidationProblemDetails(result.Errors));

    // a union struct that was never assigned (default) holds no case
    private static InvalidOperationException Uninitialized() =>
        new("The result holds no value. Was it created with default?");
}
