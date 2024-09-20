using JetBrains.Annotations;

namespace CraftersCloud.Core.Infrastructure;

[UsedImplicitly]
public class TimeProvider : ITimeProvider
{
    private readonly Lazy<DateTimeOffset> _now = new(() => DateTimeOffset.UtcNow);
    public DateTimeOffset UtcNow => _now.Value;
}