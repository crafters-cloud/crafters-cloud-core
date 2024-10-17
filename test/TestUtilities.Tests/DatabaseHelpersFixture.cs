using CraftersCloud.Core.TestUtils.Database;
using FluentAssertions;
using NUnit.Framework;

namespace CraftersCloud.Core.TestUtils.Tests;

[Category("unit")]
public class DatabaseHelpersFixture
{
    [Test]
    public void TestDropAllSql()
    {
        var sql = DatabaseHelpers.DropAllSql;

        sql.Should().NotBeNullOrEmpty();
    }
}