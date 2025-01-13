namespace CraftersCloud.Core.Infrastructure;

[PublicAPI]
public class TimeProvider : ITimeProvider
{
    private readonly Lazy<DateTimeOffset> _fixedNow = new(() => DateTimeOffset.UtcNow);

    public DateTimeOffset FixedUtcNow => _fixedNow.Value;
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}