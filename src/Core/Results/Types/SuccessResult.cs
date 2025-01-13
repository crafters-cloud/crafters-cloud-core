namespace CraftersCloud.Core.Results.Types;

public class SuccessResult<T>(T value, string successMessage) : IValueResult<T>
{
    public T Value { get; } = value;
    public string SuccessMessage { get; } = successMessage;
}