using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework.Seeding;

/// <summary>
/// Base interface for seeding data using the ModelBuilder (i.e. which will produce migrations).
/// </summary>
[PublicAPI]
public interface IModelBuilderSeeding
{
    void Seed(ModelBuilder modelBuilder);
}