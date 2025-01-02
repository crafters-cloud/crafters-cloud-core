namespace CraftersCloud.Core.Results;

public class ErrorResult : IErrorResult<string>
{
    public IEnumerable<string> Errors { get; init; } = [];
}