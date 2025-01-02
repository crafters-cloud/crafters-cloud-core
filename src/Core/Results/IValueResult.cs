namespace CraftersCloud.Core.Results;

public interface IValueResult<out T> : IResult
{
    T Value { get; }
}