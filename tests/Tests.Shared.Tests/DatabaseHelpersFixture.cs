using CraftersCloud.Core.Tests.Shared.Database;
using FluentAssertions;

namespace CraftersCloud.Core.TestUtilities.Tests;

[Category("unit")]
public class DatabaseHelpersFixture
{
    [Test]
    public void DropAllSql()
    {
        var sql = DatabaseHelpers.DropAllSql;

        sql.Should().NotBeNullOrEmpty();
    }
}