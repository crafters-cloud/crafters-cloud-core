using CraftersCloud.Core.Tests.Shared.Database;

namespace CraftersCloud.Core.TestUtilities.Tests;

[Category("unit")]
public class DatabaseHelpersFixture
{
    [Test]
    public void DropAllSqlServer()
    {
        var sql = DatabaseHelpers.DropAllSqlServerScript;

        sql.ShouldNotBeNullOrEmpty();
    }
    
    [Test]
    public void DropAllPostgreSql()
    {
        var sql = DatabaseHelpers.DropAllPostgreSqlScript;

        sql.ShouldNotBeNullOrEmpty();
    }
}