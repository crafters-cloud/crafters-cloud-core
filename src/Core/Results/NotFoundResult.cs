namespace CraftersCloud.Core.Results;

public class NotFoundResult : IErrorResult<string>
{
    public IEnumerable<string> Errors { get; init; } = [];
}