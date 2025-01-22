using System.Text.Json;
using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.SystemTextJson.Tests;

[Category("unit")]
public class StronglyTypedIdJsonConverterFixture
{
    private readonly TestStronglyTypedId ATestId = TestStronglyTypedId.Create(Guid.NewGuid());
    private TestIdContainer AContainer => new() { Id = ATestId };

    [Test]
    public void GivenId_Serialize()
    {
        var serialized = Serialize(ATestId);
        serialized.ShouldBe($"\"{ATestId.Value}\"");
    }

    [Test]
    public void GivenId_Deserialize()
    {
        var deserialized = Deserialize<TestStronglyTypedId>($"\"{ATestId.Value}\"");
        deserialized.ShouldBe(ATestId);
    }

    [Test]
    public void GivenContainer_WithId_Serialize()
    {
        var serialized = Serialize(AContainer);
        serialized.ShouldBe($"{{\"Id\":\"{ATestId.Value}\"}}");
    }

    [Test]
    public void GivenContainer_WithoutId_Serialize()
    {
        var serialized = Serialize(new TestIdContainer { Id = null });
        serialized.ShouldBe($"{{\"Id\":null}}");
    }

    [Test]
    public void GivenContainer_Deserialize()
    {
        var deserialized = Deserialize<TestIdContainer>($"{{\"Id\":\"{ATestId.Value}\"}}");
        deserialized.ShouldNotBeNull();
        deserialized.Id.ShouldBe(ATestId);
    }

    [TestCase(null, typeof(ArgumentNullException))]
    [TestCase("", typeof(JsonException))]
    public void DeserializeEmptyThrowsError(string? json, Type expectedExceptionType) =>
        Should.Throw(() => Deserialize<TestStronglyTypedId>(json!), expectedExceptionType);
    
    private static string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    private static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, Options);

    private static JsonSerializerOptions Options => new()
    {
        Converters = { new StronglyTypedIdJsonConverter<TestStronglyTypedId, Guid>() }
    };
}

[StronglyTypedId(ValueKind.Guid)]
public readonly partial record struct TestStronglyTypedId;

public class TestIdContainer
{
    public TestStronglyTypedId? Id { get; set; }
}