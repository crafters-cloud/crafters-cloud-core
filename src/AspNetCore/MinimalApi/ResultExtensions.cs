using CraftersCloud.Core.Results;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using OneOf;
using NoContentResult = CraftersCloud.Core.Results.NoContentResult;
using NotFoundResult = CraftersCloud.Core.Results.NotFoundResult;

namespace CraftersCloud.Core.AspNetCore.MinimalApi;

[PublicAPI]
public static class ResultExtensions
{
    public static Results<Created<T>, BadRequest> ToMinimalApiResult<T>(
        this OneOfBase<CreatedResult<T>, InvalidResult> command) =>
        command.Match<Results<Created<T>, BadRequest>>(
            created => TypedResults.Created("", created.Value),
            badRequest => TypedResults.BadRequest());

    public static Results<NoContent, NotFound, BadRequest>
        ToMinimalApiResult(this OneOfBase<NoContentResult, NotFoundResult, InvalidResult> command) =>
        command.Match<Results<NoContent, NotFound, BadRequest>>(
            noContent => TypedResults.NoContent(),
            notFound => TypedResults.NotFound(),
            badRequest => TypedResults.BadRequest());
}