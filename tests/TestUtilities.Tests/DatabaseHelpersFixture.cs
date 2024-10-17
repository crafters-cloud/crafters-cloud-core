using CraftersCloud.Core.TestUtilities.Database;
using FluentAssertions;
using NUnit.Framework;

namespace CraftersCloud.Core.TestUtils.Tests;

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