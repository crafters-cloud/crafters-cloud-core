using CraftersCloud.Core.StronglyTypedIds;
using FluentAssertions;

namespace CraftersCloud.Core.SourceGenerator.Tests;

[Category("unit")]
public class GuidValueStronglyTypedIdGeneratorFixture
{
    private readonly GuidId _id = GuidId.New();
    
    [Test]
    public void GetValue()
    {
        _id.Value.Should().NotBe(Guid.Empty);
    }

    [Test]
    public void ImplementsStronglyTypedIdInterface()
    {
        _id.Should().BeAssignableTo<IStronglyTypedId<Guid>>();
    }

    [Test]
    public void GivenInvalidGuid_TryParse_ReturnsFalse()
    {
        GuidId.TryParse("", out GuidId _).Should().BeFalse();
        GuidId.TryParse("something", out GuidId _).Should().BeFalse();
    }
    
    [Test]
    public void GivenValidGuid_TryParse_ReturnsTrue()
    {
        var aGuidString = "0BAD7F44-38BC-42E2-815D-280389D7093E";
        GuidId.TryParse(aGuidString, out GuidId id).Should().BeTrue();
        id.Value.Should().Be(Guid.Parse(aGuidString));
    }
    
    [Test]
    public void ImplicitOperator()
    {
        Guid valueConverted = _id;
        valueConverted.Should().Be(_id.Value);
    }
    
    [Test]
    public void TestToString()
    {
        _id.ToString().Should().Be(_id.Value.ToString());
    }
}

[StronglyTypedId(ValueKind.Guid)]
public partial record struct GuidId;