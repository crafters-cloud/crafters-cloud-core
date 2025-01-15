using CraftersCloud.Core.Helpers;

namespace CraftersCloud.Core.Tests.Shared.Database;

[PublicAPI]
public static class DatabaseHelpers
{
    public static string DropAllSql =>
        EmbeddedResource.ReadResourceContent(typeof(DatabaseHelpers).Assembly,
            "CraftersCloud.Core.Tests.Shared.Database.DropAllSql.sql");
}