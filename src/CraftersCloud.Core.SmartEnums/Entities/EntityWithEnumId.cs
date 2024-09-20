using Ardalis.SmartEnum;
using CraftersCloud.Core.Entities;

namespace CraftersCloud.Core.SmartEnums.Entities;

/// <summary>
/// Base class for entities with enum id. Uses Ardalis.SmartEnum for id.
/// </summary>
/// <typeparam name="TId">Type of id</typeparam>
public abstract class EntityWithEnumId<TId> : EntityWithTypedId<TId> where TId : SmartEnum<TId>
{
    public string Name { get; private init; } = string.Empty;
}
