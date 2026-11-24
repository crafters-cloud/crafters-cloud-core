using CraftersCloud.Core.MediatR;
using CraftersCloud.Core.Results;
using FluentValidation;
using MediatR;

namespace CraftersCloud.Core.Tests.MediatR;

[Category("unit")]
public class ValidationBehaviorFixture
{
    public sealed record Command(string Name) : IRequest<UpdateResult>, IRequest<string>;

    private sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => RuleFor(x => x.Name).NotEmpty();
    }

    [Test]
    public async Task GivenValidRequest_CallsTheHandler()
    {
        var behavior = new ValidationBehavior<Command, UpdateResult>([new CommandValidator()]);

        var result = await behavior.Handle(new Command("x"), _ => Task.FromResult<UpdateResult>(Result.NoContent()),
            CancellationToken.None);

        result.Value.ShouldBeOfType<NoContentResult>();
    }

    [Test]
    public async Task GivenInvalidRequest_AndUnionWithBadRequest_ReturnsBadRequest()
    {
        var behavior = new ValidationBehavior<Command, UpdateResult>([new CommandValidator()]);

        var result = await behavior.Handle(new Command(""), _ => throw new InvalidOperationException("not called"),
            CancellationToken.None);

        var error = result.Value.ShouldBeOfType<BadRequestResult>().Errors.ShouldHaveSingleItem();
        error.PropertyName.ShouldBe("Name");
        error.Code.ShouldBe("NotEmptyValidator");
        error.Message.ShouldNotBeNullOrEmpty();
    }

    [Test]
    public async Task GivenInvalidRequest_AndResponseWithoutBadRequest_Throws()
    {
        var behavior = new ValidationBehavior<Command, string>([new CommandValidator()]);

        await Should.ThrowAsync<ValidationException>(() =>
            behavior.Handle(new Command(""), _ => Task.FromResult("ok"), CancellationToken.None));
    }
}
