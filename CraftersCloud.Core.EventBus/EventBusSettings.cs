using JetBrains.Annotations;

namespace CraftersCloud.Core.EventBus;

[UsedImplicitly]
public class EventBusSettings
{
    public bool Enabled { get; set; }
    public string SubscriptionClientName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}