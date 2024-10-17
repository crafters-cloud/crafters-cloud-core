using CraftersCloud.Core.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[UsedImplicitly]
public abstract class EntitiesDbContext(
    EntitiesDbContextOptions entitiesDbContextOptions,
    DbContextOptions options)
    : DbContext(options)
{
    private EntitiesDbContextOptions EntitiesDbContextOptions { get; init; } = entitiesDbContextOptions;
    public Action<ModelBuilder>? ModelBuilderConfigurator { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(EntitiesDbContextOptions.ConfigurationAssembly);

        RegisterEntities(modelBuilder);

        ModelBuilderConfigurator?.Invoke(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void RegisterEntities(ModelBuilder modelBuilder)
    {
        var entityMethod =
            typeof(ModelBuilder).GetMethods().First(m => m.Name == "Entity" && m.IsGenericMethod);

        var entitiesAssembly = EntitiesDbContextOptions.EntitiesAssembly;
        var types = entitiesAssembly?.GetTypes() ?? Enumerable.Empty<Type>();

        var entityTypes = types
            .Where(x => x.IsSubclassOf(typeof(Entity)) && !x.IsAbstract);

        foreach (var type in entityTypes)
        {
            if (EntitiesDbContextOptions.EntityTypePredicate != null &&
                !EntitiesDbContextOptions.EntityTypePredicate(type))
            {
                continue;
            }

            entityMethod.MakeGenericMethod(type).Invoke(modelBuilder, []);
        }
    }
}