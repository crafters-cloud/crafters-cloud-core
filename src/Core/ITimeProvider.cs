namespace CraftersCloud.Core;

/// <summary>
/// Represents abstraction over time management. Makes any time-based code easily testable.
/// </summary>
[PublicAPI]
public interface ITimeProvider
{
    /// <summary>
    /// Returns current UTC time for the first invocation (within the definec lifetime scope). Subsequent invocations return cached value (the same value as in first invocation).
    /// This is useful in the scenarios when you want exact same time value to be returned within the scope (e.g. within the single HTTP request).
    /// </summary>
    DateTimeOffset FixedUtcNow { get; }

    /// <summary>
    /// Always returns current UTC time, no matter how many times you invoke it.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}