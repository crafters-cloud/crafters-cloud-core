using CraftersCloud.Core.Results;
using OneOf;

namespace CraftersCloud.Core.Cqrs.CommandResults;

public static class CommandResult
{
    public static CreateCommandResult<T> Created<T>(T value) => new(Result.Created(value));
    public static UpdateCommandResult<T> UpdateSuccess<T>() => new(Result.NoContent());
    public static UpdateCommandResult<T> UpdateNotFound<T>() => new(Result.NotFound());
}

[GenerateOneOf]
public partial class CreateCommandResult<T> : OneOfBase<CreatedResult<T>, InvalidResult>;

[GenerateOneOf]
public partial class UpdateCommandResult<T> : OneOfBase<NoContentResult, NotFoundResult, InvalidResult>;