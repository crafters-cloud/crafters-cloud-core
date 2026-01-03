using CraftersCloud.Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.Tests.Shared.Database;

[PublicAPI]
public static class DbContextExtensions
{
    extension(DbContext context)
    {
        public bool IsSqlServer() => context.Database.ProviderName.ToEmptyIfNull().Contains("SqlServer", StringComparison.InvariantCultureIgnoreCase);

        public bool IsPostgres() => context.Database.ProviderName.ToEmptyIfNull().Contains("PostgreSQL", StringComparison.InvariantCultureIgnoreCase);
    }
}