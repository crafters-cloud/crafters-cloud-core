namespace CraftersCloud.Core.Results;

public interface IErrorResult<out T> : IResult
{
    IEnumerable<T> Errors { get; }
}