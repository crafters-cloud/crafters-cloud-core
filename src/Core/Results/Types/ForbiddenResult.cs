namespace CraftersCloud.Core.Results.Types;

public class ForbiddenResult : IErrorResult<string>
{
    public IEnumerable<string> Errors { get; init; } = [];
}