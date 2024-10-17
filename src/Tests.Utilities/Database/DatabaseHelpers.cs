using CraftersCloud.Core.Helpers;
using JetBrains.Annotations;

namespace CraftersCloud.Core.TestUtilities.Database;

[PublicAPI]
public static class DatabaseHelpers
{
    public static string DropAllSql =>
        EmbeddedResource.ReadResourceContent(typeof(DatabaseHelpers).Assembly,
            "CraftersCloud.Core.TestUtilities.Database.DropAllSql.sql");
}