namespace CraftersCloud.Core.Results;

public class ForbiddenResult : IErrorResult<string>
{
    public IEnumerable<string> Errors { get; init; } = [];
}