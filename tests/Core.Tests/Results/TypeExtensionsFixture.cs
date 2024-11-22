using CraftersCloud.Core.Results;
using FluentAssertions;
using JetBrains.Annotations;
using NUnit.Framework;
using OneOf;

namespace CraftersCloud.Core.Tests.Results;

[Category("unit")]
public class TypeExtensionsFixture
{
    [Test]
    public void GivenClassWithInvalidResult_HasInvalidResultType_Returns_True()
    {
        var result = typeof(AClassWithInvalidResult);
        result.IsDerivedFromOneOfType<InvalidResult>().Should().BeTrue();
    }

    [Test]
    public void GivenClassWithoutInvalidResult_HasInvalidResultType_Returns_False()
    {
        var result = typeof(AClassWithoutInvalidResult);
        result.IsDerivedFromOneOfType<InvalidResult>().Should().BeFalse();
    }

    [Test]
    public void GivenClassWithInvalidResult_MapToOneOf_DoesMapping()
    {
        var invalidResult = new InvalidResult([]);
        var result = invalidResult.MapToOneOf<AClassWithInvalidResult>();
        result.Value.Should().Be(invalidResult);
    }

    [Test]
    public void GivenClassWithoutInvalidResult_MapToOneOf_ThrowsAnException()
    {
        var invalidResult = new InvalidResult([]);
        var resultFunc = () => invalidResult.MapToOneOf<AClassWithoutInvalidResult>();
        resultFunc.Should().Throw<InvalidOperationException>().WithMessage(
            "No implicit conversion operator found. Maybe 'AClassWithoutInvalidResult' class does not inherits from OneOfBase with 'InvalidResult'.");
    }

    [UsedImplicitly]
    public class AClass;

    [GenerateOneOf]
    private class AClassWithInvalidResult(OneOf<AClass, InvalidResult> input) : OneOfBase<AClass, InvalidResult>(input);

    [GenerateOneOf]
    private class AClassWithoutInvalidResult(OneOf<AClass, NotFoundResult> input)
        : OneOfBase<AClass, NotFoundResult>(input);
}