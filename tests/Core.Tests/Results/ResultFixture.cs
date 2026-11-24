using CraftersCloud.Core.Results;

namespace CraftersCloud.Core.Tests.Results;

[Category("unit")]
public class ResultFixture
{
    private sealed record Dto(int Id);

    private sealed record Overlap(DateOnly Start, DateOnly End);

    private sealed record ApartmentNotFound(int ApartmentId);

    private union ReserveError(ApartmentNotFound, Overlap);

    private static QueryResult<Dto> Find(int id) => id > 0 ? new Dto(id) : Result.NotFound();

    private static string Describe(QueryResult<Dto> result) => result switch
    {
        Dto dto => $"dto {dto.Id}",
        NotFoundResult => "not found",
        null => "empty"
    };

    [Test]
    public void QueryResult_HoldsTheValueOrNotFound()
    {
        Describe(Find(1)).ShouldBe("dto 1");
        Describe(Find(0)).ShouldBe("not found");
        Describe(default).ShouldBe("empty");
    }

    [Test]
    public void FactoryCases_ConvertToTheStandardUnions()
    {
        CreateResult<Dto> created = Result.Created(new Dto(1));
        created.Value.ShouldBe(new CreatedResult<Dto>(new Dto(1)));

        UpdateResult noContent = Result.NoContent();
        noContent.Value.ShouldBeOfType<NoContentResult>();

        UpdateResult badRequest = Result.BadRequest("name", "Required", "NotEmptyValidator");
        badRequest.Value.ShouldBeOfType<BadRequestResult>()
            .Errors.ShouldBe([new ValidationError("name", "Required", "NotEmptyValidator")]);
    }

    [Test]
    public void CommandResult_HoldsTheUseCaseFailures()
    {
        CommandResult<Dto, ReserveError> result = (ReserveError) new Overlap(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 2));

        var description = result switch
        {
            Dto => "reserved",
            ReserveError error => error switch
            {
                ApartmentNotFound => "apartment not found",
                Overlap => "overlap",
                null => "empty"
            },
            BadRequestResult => "bad request",
            null => "empty"
        };

        description.ShouldBe("overlap");
    }

    [Test]
    public void Errors_DefaultToEmpty()
    {
        Result.NotFound().Errors.ShouldBeEmpty();
        Result.Forbidden("no access").Errors.ShouldBe(["no access"]);
        Result.Error("a", "b").Errors.ShouldBe(["a", "b"]);
    }

    [Test]
    public void IsFailure_RecognizesTheFailureCases()
    {
        Result.IsFailure(Result.NotFound()).ShouldBeTrue();
        Result.IsFailure(Result.Forbidden()).ShouldBeTrue();
        Result.IsFailure(Result.Error()).ShouldBeTrue();
        Result.IsFailure(Result.BadRequest()).ShouldBeTrue();
        Result.IsFailure(Result.NoContent()).ShouldBeFalse();
        Result.IsFailure(Result.Created(1)).ShouldBeFalse();
        Result.IsFailure(new Dto(1)).ShouldBeFalse();
        Result.IsFailure(null).ShouldBeFalse();
    }
}
