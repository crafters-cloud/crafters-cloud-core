using CraftersCloud.Core.Messaging.CommandResults;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CraftersCloud.Core.AspNetCore.MinimalApi;

[PublicAPI]
public static class CommandResultExtensions
{
    public static Results<Created<T>, BadRequest> ToMinimalApiResult<T>(
        this CreateCommandResult<T> command) =>
        command.Match<Results<Created<T>, BadRequest>>(
            created => TypedResults.Created("", created.Value),
            badRequest => TypedResults.BadRequest());

    public static Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>
        ToMinimalApiResult<T>(this UpdateCommandResult<T> command) =>
        command.Match<Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>>(
            noContent => TypedResults.NoContent(),
            notFound => TypedResults.NotFound(),
            badRequest =>
                TypedResults.BadRequest(
                    ValidationProblemDetailsMapper.CreateValidationProblemDetails(string.Empty,
                        badRequest.ValidationFailures)));
}