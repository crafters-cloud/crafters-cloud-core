namespace CraftersCloud.Core.Results.Types;

public class ErrorResult : IErrorResult<string>
{
    public IEnumerable<string> Errors { get; init; } = [];
}