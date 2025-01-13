namespace CraftersCloud.Core.Results;

public class CreatedResult<T>(T value) : IValueResult<T>
{
    public T Value { get; } = value;
}