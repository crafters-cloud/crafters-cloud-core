using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.EntityFramework.Seeding;

/// <summary>
/// Base interface for seeding data using the DbContext (i.e. this will not produce migrations).
/// </summary>
[PublicAPI]
public interface IDbContextSeeding<in T> where T : DbContext
{
    void Seed(T dbContext);
}