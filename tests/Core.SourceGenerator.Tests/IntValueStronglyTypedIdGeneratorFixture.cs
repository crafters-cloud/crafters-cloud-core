using CraftersCloud.Core.StronglyTypedIds;
using FluentAssertions;

namespace CraftersCloud.Core.SourceGenerator.Tests;

[Category("unit")]
public class IntValueStronglyTypedIdGeneratorFixture
{
    private IntId _id = IntId.New();
    
    [Test]
    public void GetValue()
    {
        _id.Value.Should().Be(0);
    }

    [Test]
    public void ImplementsStronglyTypedIdInterface()
    {
        _id.Should().BeAssignableTo<IStronglyTypedId<int>>();
    }

    [Test]
    public void GivenInvalidInt_TryParse_ReturnsFalse()
    {
        IntId.TryParse("", out IntId _).Should().BeFalse();
        IntId.TryParse("something", out IntId _).Should().BeFalse();
    }
    
    [Test]
    public void GivenValidInt_TryParse_ReturnsTrue()
    {
        var anIntString = "23";
        IntId.TryParse(anIntString, out IntId id).Should().BeTrue();
        id.Value.Should().Be(int.Parse(anIntString));
    }
    
    [Test]
    public void ImplicitOperator()
    {
        int valueConverted = _id;
        valueConverted.Should().Be(_id.Value);
    }
    
    [Test]
    public void TestToString()
    {
        _id.ToString().Should().Be(_id.Value.ToString());
    }
}

[StronglyTypedId(ValueKind.Int)]
public partial record struct IntId;