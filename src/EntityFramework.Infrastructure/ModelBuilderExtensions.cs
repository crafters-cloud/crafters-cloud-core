using CraftersCloud.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[PublicAPI]
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Register entities from the assembly specified in the options
    /// </summary>
    /// <param name="modelBuilder">Model Builder</param>
    /// <param name="options">Registration options</param>
    public static void RegisterEntities(this ModelBuilder modelBuilder, EntityRegistrationOptions options)
    {
        var entitiesAssembly = options.EntitiesAssembly;
        var types = entitiesAssembly.GetTypes();

        var entityTypes = types
            .Where(x => x.IsSubclassOf(typeof(Entity)) && !x.IsAbstract);

        foreach (var type in entityTypes)
        {
            if (options.EntityTypePredicate != null && !options.EntityTypePredicate(type))
            {
                // if predicate is defined and returns false, skip the type
                continue;
            }

            modelBuilder.Entity(type);
        }
    }
}