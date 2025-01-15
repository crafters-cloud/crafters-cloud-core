using CraftersCloud.Core.Results;
using OneOf;

namespace CraftersCloud.Core.Tests.Results;

[Category("unit")]
public class TypeExtensionsFixture
{
    [Test]
    public void GivenClassWithInvalidResult_HasInvalidResultType_Returns_True()
    {
        var result = typeof(AClassWithBadRequestResult);
        result.IsDerivedFromOneOfType<BadRequestResult>().ShouldBeTrue();
    }

    [Test]
    public void GivenClassWithoutInvalidResult_HasInvalidResultType_Returns_False()
    {
        var result = typeof(AClassWithoutBadRequestResult);
        result.IsDerivedFromOneOfType<BadRequestResult>().ShouldBeFalse();
    }

    [Test]
    public void GivenClassWithInvalidResult_MapToOneOf_DoesMapping()
    {
        var invalidResult = new BadRequestResult([]);
        var result = invalidResult.MapToOneOf<AClassWithBadRequestResult>();
        result.Value.ShouldBe(invalidResult);
    }

    [Test]
    public void GivenClassWithoutInvalidResult_MapToOneOf_ThrowsAnException()
    {
        var invalidResult = new BadRequestResult([]);
        var resultFunc = () => invalidResult.MapToOneOf<AClassWithoutBadRequestResult>();
        resultFunc.ShouldThrow<InvalidOperationException>().Message.ShouldBe(
            "No implicit conversion operator found. Maybe 'AClassWithoutBadRequestResult' class does not inherits from OneOfBase with 'BadRequestResult'.");
    }
}

[UsedImplicitly]
public class AClass;

[GenerateOneOf]
public partial class AClassWithBadRequestResult : OneOfBase<AClass, BadRequestResult>;

[GenerateOneOf]
public partial class AClassWithoutBadRequestResult : OneOfBase<AClass, NotFoundResult>;