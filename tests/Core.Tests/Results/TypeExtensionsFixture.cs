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
        var result = typeof(AClassWithBadRequestResult);
        result.IsDerivedFromOneOfType<BadRequestResult>().Should().BeTrue();
    }

    [Test]
    public void GivenClassWithoutInvalidResult_HasInvalidResultType_Returns_False()
    {
        var result = typeof(AClassWithoutBadRequestResult);
        result.IsDerivedFromOneOfType<BadRequestResult>().Should().BeFalse();
    }

    [Test]
    public void GivenClassWithInvalidResult_MapToOneOf_DoesMapping()
    {
        var invalidResult = new BadRequestResult([]);
        var result = invalidResult.MapToOneOf<AClassWithBadRequestResult>();
        result.Value.Should().Be(invalidResult);
    }

    [Test]
    public void GivenClassWithoutInvalidResult_MapToOneOf_ThrowsAnException()
    {
        var invalidResult = new BadRequestResult([]);
        var resultFunc = () => invalidResult.MapToOneOf<AClassWithoutBadRequestResult>();
        resultFunc.Should().Throw<InvalidOperationException>().WithMessage(
            "No implicit conversion operator found. Maybe 'AClassWithoutBadRequestResult' class does not inherits from OneOfBase with 'BadRequestResult'.");
    }
}

[UsedImplicitly]
public class AClass;

[GenerateOneOf]
public partial class AClassWithBadRequestResult : OneOfBase<AClass, BadRequestResult>;

[GenerateOneOf]
public partial class AClassWithoutBadRequestResult : OneOfBase<AClass, NotFoundResult>;