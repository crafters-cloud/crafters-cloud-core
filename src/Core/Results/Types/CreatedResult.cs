namespace CraftersCloud.Core.Results.Types;

public class CreatedResult<T>(T value) : IValueResult<T>
{
    public T Value { get; } = value;
}