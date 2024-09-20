using JetBrains.Annotations;

namespace CraftersCloud.Core.Infrastructure
{
    [PublicAPI]
    public class TimeProvider : ITimeProvider
    {
        private readonly Lazy<DateTimeOffset> _now = new(() => DateTimeOffset.UtcNow);

        public DateTimeOffset FixedUtcNow => _now.Value;
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}