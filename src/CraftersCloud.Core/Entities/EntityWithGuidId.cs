namespace CraftersCloud.Core.Entities
{
    public abstract class EntityWithGuidId : EntityWithTypedId<Guid>
    {
        protected EntityWithGuidId()
        {
            Id = SequentialGuidGenerator.Generate();
        }
    }
}
