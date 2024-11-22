using CraftersCloud.Core.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using BadRequestResult = CraftersCloud.Core.Results.BadRequestResult;
using NoContentResult = CraftersCloud.Core.Results.NoContentResult;
using NotFoundResult = CraftersCloud.Core.Results.NotFoundResult;

namespace CraftersCloud.Core.AspNetCore.MinimalApi;

[PublicAPI]
public static class ResultExtensions
{
    public static Results<Created<T>, BadRequest> ToMinimalApiResult<T>(
        this OneOfBase<CreatedResult<T>, BadRequestResult> command) =>
        command.Match<Results<Created<T>, BadRequest>>(
            created => TypedResults.Created("", created.Value),
            badRequest => TypedResults.BadRequest());

    public static Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>
        ToMinimalApiResult<T>(this OneOfBase<NoContentResult, NotFoundResult, BadRequestResult> command) =>
        command.Match<Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>>(
            noContent => TypedResults.NoContent(),
            notFound => TypedResults.NotFound(),
            badRequest =>
                TypedResults.BadRequest(
                    ValidationProblemDetailsMapper.CreateValidationProblemDetails(string.Empty,
                        badRequest.ValidationFailures)));
}