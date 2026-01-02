using CraftersCloud.Core.Helpers;

namespace CraftersCloud.Core.Tests.Shared.Database;

[PublicAPI]
public static class DatabaseHelpers
{
    public static string DropAllSqlServerScript =>
        EmbeddedResource.ReadResourceContent(typeof(DatabaseHelpers).Assembly,
            "CraftersCloud.Core.Tests.Shared.Database.DropAllSqlServer.sql");

    public static string DropAllPostgreSqlScript =>
        EmbeddedResource.ReadResourceContent(typeof(DatabaseHelpers).Assembly,
            "CraftersCloud.Core.Tests.Shared.Database.DropAllPostgreSql.sql");
}