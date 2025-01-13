using CraftersCloud.Core.Entities;

namespace CraftersCloud.Core.IntegrationEvents;

[PublicAPI]
public class IntegrationEvent
{
    public Guid Id { get; set; } = SequentialGuidGenerator.Generate();
    public DateTimeOffset CreationDate { get; set; } = DateTimeOffset.UtcNow;
}