﻿namespace CraftersCloud.Core.Entities;

public abstract class EntityWithTypedId<TId> : Entity
{
    public TId Id { get; set; } = default!;
}