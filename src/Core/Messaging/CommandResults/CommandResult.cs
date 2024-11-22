using CraftersCloud.Core.Results;
using JetBrains.Annotations;
using OneOf;

namespace CraftersCloud.Core.Messaging.CommandResults;

[PublicAPI]
public static class CommandResult
{
    public static CreateCommandResult<T> Created<T>(T value) => new(Result.Created(value));
    public static UpdateCommandResult<T> UpdateSuccess<T>() => new(Result.NoContent());
    public static UpdateCommandResult<T> UpdateNotFound<T>() => new(Result.NotFound());
}

[PublicAPI]
[GenerateOneOf]
public partial class CreateCommandResult<T> : OneOfBase<CreatedResult<T>, InvalidResult>;

[PublicAPI]
[GenerateOneOf]
public partial class UpdateCommandResult<T> : OneOfBase<NoContentResult, NotFoundResult, InvalidResult>;