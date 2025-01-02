using FluentValidation.Results;

namespace CraftersCloud.Core.Results;

public class BadRequestResult(IReadOnlyCollection<ValidationFailure> errors) : IErrorResult<ValidationFailure>
{
    public IEnumerable<ValidationFailure> Errors { get; } = errors;
}