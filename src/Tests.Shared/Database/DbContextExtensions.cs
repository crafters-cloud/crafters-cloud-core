using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.Core.Tests.Shared.Database;

[PublicAPI]
public static class DbContextExtensions
{
    extension(DbContext context)
    {
        public bool IsSqlServer() => context.Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer";

        public bool IsPostgres() => context.Database.ProviderName == "Microsoft.EntityFrameworkCore.PostgreSQL";
    }
}