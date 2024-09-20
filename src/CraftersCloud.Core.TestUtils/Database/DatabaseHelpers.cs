using JetBrains.Annotations;

namespace CraftersCloud.Core.TestUtils.Database;

[PublicAPI]
public static class DatabaseHelpers
{
    public static string DropAllSql =>
        EmbeddedResource.ReadResourceContent(typeof(DatabaseHelpers).Assembly,
            "CraftersCloud.Core.TestUtils.Database.DropAllSql.sql");
}