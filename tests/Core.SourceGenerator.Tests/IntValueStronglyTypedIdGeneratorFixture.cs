using CraftersCloud.Core.StronglyTypedIds;

namespace CraftersCloud.Core.SourceGenerator.Tests;

[Category("unit")]
public class IntValueStronglyTypedIdGeneratorFixture
{
    private readonly IntId _id = IntId.Create(10);

    [Test]
    public void GetValue() => _id.Value.ShouldBe(10);

    [Test]
    public void ImplementsStronglyTypedIdInterface() => _id.ShouldBeAssignableTo<IStronglyTypedId<int>>();

    [Test]
    public void GivenInvalidInt_TryParse_ReturnsFalse()
    {
        IntId.TryParse("", out _).ShouldBeFalse();
        IntId.TryParse("something", out _).ShouldBeFalse();
    }

    [Test]
    public void GivenValidInt_TryParse_ReturnsTrue()
    {
        var anIntString = "23";
        IntId.TryParse(anIntString, out var id).ShouldBeTrue();
        id.Value.ShouldBe(int.Parse(anIntString));
    }

    [Test]
    public void ImplicitOperator()
    {
        int valueConverted = _id;
        valueConverted.ShouldBe(_id.Value);
    }

    [Test]
    public void TestToString() => _id.ToString().ShouldBe(_id.Value.ToString());
}

[StronglyTypedId(ValueKind.Int)]
public readonly partial record struct IntId;