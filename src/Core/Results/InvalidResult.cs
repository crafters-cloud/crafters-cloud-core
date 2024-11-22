using Ardalis.Result;
using FluentValidation.Results;

namespace CraftersCloud.Core.Results;

public class InvalidResult(IReadOnlyCollection<ValidationFailure> validationFailures) : Ardalis.Result.Result(ResultStatus.Invalid)
{
    public IReadOnlyCollection<ValidationFailure> ValidationFailures { get; } = validationFailures;
}