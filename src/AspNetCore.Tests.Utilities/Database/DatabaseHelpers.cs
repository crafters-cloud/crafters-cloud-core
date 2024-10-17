using JetBrains.Annotations;

namespace CraftersCloud.Core.AspNetCore.Tests.Utilities.Database;

[UsedImplicitly]
public static class DatabaseHelpers
{
    public static string DropAllSql =>
        EmbeddedResource.ReadResourceContent(
            typeof(DatabaseHelpers).Assembly, $"{typeof(DatabaseHelpers).Namespace}.DropAllSql.sql");
}