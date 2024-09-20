namespace CraftersCloud.Core;

public interface ITimeProvider
{
    DateTimeOffset UtcNow { get; }
}