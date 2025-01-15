using CraftersCloud.Core.Collections;

namespace CraftersCloud.Core.Tests;

[Category("unit")]
public class CollectionUpdaterExtensionsFixture
{
    [Test]
    public void UpdateWith_AddsNewItems()
    {
        var collection = new List<string>();
        var values = new List<string> { "a", "b" };

        collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { });

        collection.Count.ShouldBe(2);
        collection.ShouldContain("a");
        collection.ShouldContain("b");
    }

    [Test]
    public void UpdateWith_UpdatesExistingItems()
    {
        var collection = new List<string> { "a" };
        var values = new List<string> { "a" };

        collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { });

        collection.Count.ShouldBe(1);
        collection.ShouldContain("a");
    }

    [Test]
    public void UpdateWithoutRemove_DoesNotRemoveItems()
    {
        var collection = new List<string> { "a", "b" };
        var values = new List<string> { "a" };

        collection.UpdateWithoutRemove(values, (v, c) => v == c, v => v, (v, c) => { });

        collection.Count.ShouldBe(2);
        collection.ShouldContain("a");
        collection.ShouldContain("b");
    }

    [Test]
    public void AddOrUpdate_AddsNewItem()
    {
        var collection = new List<string>();
        const string value = "a";

        var result = collection.AddOrUpdate(value, c => c == value, v => v, (v, c) => { });

        collection.Count.ShouldBe(1);
        result.ShouldContain("a");
    }

    [Test]
    public void AddOrUpdate_UpdatesExistingItem()
    {
        var collection = new List<string> { "a" };
        const string value = "a";

        var result = collection.AddOrUpdate(value, c => c == value, v => v, (v, c) => { });

        collection.Count.ShouldBe(1);
        result.ShouldBe("a");
    }

    [Test]
    public void UpdateWith_NullDeleter_DoesNotThrow()
    {
        var collection = new List<string> { "a" };
        var values = new List<string> { "a" };

        Action act = () => collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { }, null);

        act.ShouldNotThrow();
    }

    [Test]
    public void UpdateWith_RemovesItems()
    {
        var collection = new List<string> { "a", "b" };
        var values = new List<string> { "a" };

        collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { }, c => collection.Remove(c));

        collection.Count.ShouldBe(1);
        collection.ShouldContain("a");
        collection.ShouldNotContain("b");
    }

    [Test]
    public void UpdateWith_EmptyValues_DoesNotThrow()
    {
        var collection = new List<string> { "a" };
        var values = new List<string>();

        Action act = () => collection.UpdateWith(values, (v, c) => v == c, v => v, (v, c) => { });

        act.ShouldNotThrow();
    }

    [Test]
    public void UpdateWithoutRemove_EmptyValues_DoesNotThrow()
    {
        var collection = new List<string> { "a" };
        var values = new List<string>();

        Action act = () => collection.UpdateWithoutRemove(values, (v, c) => v == c, v => v, (v, c) => { });

        act.ShouldNotThrow();
    }
}