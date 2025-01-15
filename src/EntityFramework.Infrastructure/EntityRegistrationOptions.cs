using System.Reflection;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

/// <summary>
/// Provides options for registering the entities in the DbContext
/// </summary>
[PublicAPI]
public class EntityRegistrationOptions
{
    /// <summary>
    /// Assembly containing the entity type configurations to be registered with the DbContext.
    /// </summary>
    public Assembly ConfigurationAssembly { get; init; } = null!;

    /// <summary>
    /// Predicate to filter the entity type configuration types to be registered with the DbContext.
    /// </summary>
    public Func<Type, bool>? ConfigurationTypePredicate { get; init; }

    /// <summary>
    /// Assembly containing the entities to be registered with the DbContext.
    /// </summary>
    public Assembly EntitiesAssembly { get; init; } = null!;

    /// <summary>
    /// Predicate to filter the entity types to be registered with the DbContext.
    /// </summary>
    public Func<Type, bool>? EntityTypePredicate { get; init; }
}