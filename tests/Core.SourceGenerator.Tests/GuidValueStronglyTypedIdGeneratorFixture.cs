using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.SourceGenerator.Tests;

[Category("unit")]
public class GuidValueStronglyTypedIdGeneratorFixture
{
    private readonly GuidId _id = GuidId.CreateNew();

    [Test]
    public void GetValue() => _id.Value.ShouldNotBe(Guid.Empty);

    [Test]
    public void ImplementsStronglyTypedIdInterface() => _id.ShouldBeAssignableTo<IStronglyTypedId<Guid>>();

    [Test]
    public void GivenInvalidGuid_TryParse_ReturnsFalse()
    {
        GuidId.TryParse("", out _).ShouldBeFalse();
        GuidId.TryParse("something", out _).ShouldBeFalse();
    }

    [Test]
    public void GivenValidGuid_TryParse_ReturnsTrue()
    {
        const string aGuidString = "0BAD7F44-38BC-42E2-815D-280389D7093E";
        GuidId.TryParse(aGuidString, out var id).ShouldBeTrue();
        id.Value.ShouldBe(Guid.Parse(aGuidString));
    }

    [Test]
    public void ImplicitOperator()
    {
        Guid valueConverted = _id;
        valueConverted.ShouldBe(_id.Value);
    }

    [Test]
    public void TestToString() => _id.ToString().ShouldBe(_id.Value.ToString());
}

[StronglyTypedId(ValueKind.Guid)]
public readonly partial record struct GuidId;