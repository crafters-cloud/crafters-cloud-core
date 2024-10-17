using CraftersCloud.Core.Entities;
using JetBrains.Annotations;

namespace CraftersCloud.Core.IntegrationEvents;

[PublicAPI]
public class IntegrationEvent
{
    public IntegrationEvent()
    {
        Id = SequentialGuidGenerator.Generate();
        CreationDate = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
}