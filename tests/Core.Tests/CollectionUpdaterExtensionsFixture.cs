using CraftersCloud.Core.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace CraftersCloud.Core.Tests;

public class CollectionUpdaterExtensionsFixture
{
    [Test]
    public void UpdateWith_AddsNewItems()
    {
        var collection = new List<string>();
        var values = new List<string> { "a", "b" };

        collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { });

        collection.Should().HaveCount(2);
        collection.Should().Contain("a");
        collection.Should().Contain("b");
    }

    [Test]
    public void UpdateWith_UpdatesExistingItems()
    {
        var collection = new List<string> { "a" };
        var values = new List<string> { "a" };

        collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { });

        collection.Should().HaveCount(1);
        collection.Should().Contain("a");
    }

    [Test]
    public void UpdateWithoutRemove_DoesNotRemoveItems()
    {
        var collection = new List<string> { "a", "b" };
        var values = new List<string> { "a" };

        collection.UpdateWithoutRemove(values, (v, c) => v == c, v => v, (v, c) => { });

        collection.Should().HaveCount(2);
        collection.Should().Contain("a");
        collection.Should().Contain("b");
    }

    [Test]
    public void AddOrUpdate_AddsNewItem()
    {
        var collection = new List<string>();
        const string value = "a";

        var result = collection.AddOrUpdate(value, c => c == value, v => v, (v, c) => { });

        collection.Should().HaveCount(1);
        result.Should().Be("a");
    }

    [Test]
    public void AddOrUpdate_UpdatesExistingItem()
    {
        var collection = new List<string> { "a" };
        const string value = "a";

        var result = collection.AddOrUpdate(value, c => c == value, v => v, (v, c) => { });

        collection.Should().HaveCount(1);
        result.Should().Be("a");
    }

    [Test]
    public void UpdateWith_NullDeleter_DoesNotThrow()
    {
        var collection = new List<string> { "a" };
        var values = new List<string> { "a" };

        Action act = () => collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { }, null);

        act.Should().NotThrow();
    }

    [Test]
    public void UpdateWith_RemovesItems()
    {
        var collection = new List<string> { "a", "b" };
        var values = new List<string> { "a" };

        collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { }, c => collection.Remove(c));

        collection.Should().HaveCount(1);
        collection.Should().Contain("a");
        collection.Should().NotContain("b");
    }

    [Test]
    public void UpdateWith_EmptyValues_DoesNotThrow()
    {
        var collection = new List<string> { "a" };
        var values = new List<string>();

        Action act = () => collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { });

        act.Should().NotThrow();
    }

    [Test]
    public void UpdateWithoutRemove_EmptyValues_DoesNotThrow()
    {
        var collection = new List<string> { "a" };
        var values = new List<string>();

        Action act = () => collection.UpdateWithoutRemove(values, (v, c) => v == c, v => v, (v, c) => { });

        act.Should().NotThrow();
    }
}