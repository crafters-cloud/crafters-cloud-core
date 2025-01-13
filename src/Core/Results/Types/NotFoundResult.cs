namespace CraftersCloud.Core.Results.Types;

public class NotFoundResult : IErrorResult<string>
{
    public IEnumerable<string> Errors { get; init; } = [];
}