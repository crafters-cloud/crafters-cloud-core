using FluentValidation.Results;

namespace CraftersCloud.Core.Results;

public static class Result
{
    public static BadRequestResult BadRequest(IReadOnlyCollection<ValidationFailure> failures) => new(failures);
    
    public static NotFoundResult NotFound() => new();

    public static NoContentResult NoContent() => new();

    public static CreatedResult<T> Created<T>(T value) => new(value);
}