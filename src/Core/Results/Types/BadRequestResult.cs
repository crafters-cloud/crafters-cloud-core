using FluentValidation.Results;

namespace CraftersCloud.Core.Results.Types;

public class BadRequestResult(IReadOnlyCollection<ValidationFailure> errors) : IErrorResult<ValidationFailure>
{
    public IEnumerable<ValidationFailure> Errors { get; } = errors;
}