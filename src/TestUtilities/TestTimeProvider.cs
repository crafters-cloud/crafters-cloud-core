namespace CraftersCloud.Core.TestUtilities;

/// <summary>
/// Allows setting the current time for testing purposes.
/// </summary>
[PublicAPI]
public class TestTimeProvider : ITimeProvider
{
    private DateTimeOffset? _nowValue;

    private readonly Lazy<DateTimeOffset> _now = new(() => DateTimeOffset.UtcNow);
    public DateTimeOffset FixedUtcNow => _nowValue ?? _now.Value;
    public DateTimeOffset UtcNow => _nowValue ?? DateTimeOffset.UtcNow;

    public void SetNow(DateTimeOffset value) => _nowValue = value;
}