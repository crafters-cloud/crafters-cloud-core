using System.Text.Json;
using CraftersCloud.Core.Caching;
using CraftersCloud.Core.Results;

namespace CraftersCloud.Core.Tests.Caching;

[Category("unit")]
public class UnionJsonConverterFixture
{
    public sealed record Dto(int Id, string Name);

    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new UnionJsonConverterFactory() }
    };

    private static T RoundTrip<T>(T value) =>
        JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, Options), Options)!;

    [Test]
    public void QueryResult_WithValue_RoundTrips()
    {
        QueryResult<Dto> result = new Dto(1, "a");

        RoundTrip(result).Value.ShouldBe(new Dto(1, "a"));
    }

    [Test]
    public void QueryResult_WithNotFound_RoundTrips()
    {
        QueryResult<Dto> result = Result.NotFound("missing");

        RoundTrip(result).Value.ShouldBeOfType<NotFoundResult>().Errors.ShouldBe(["missing"]);
    }

    [Test]
    public void CreateResult_WithBadRequest_RoundTrips()
    {
        CreateResult<Dto> result = Result.BadRequest("name", "Required", "NotEmptyValidator");

        RoundTrip(result).Value.ShouldBeOfType<BadRequestResult>()
            .Errors.ShouldBe([new ValidationError("name", "Required", "NotEmptyValidator")]);
    }

    [Test]
    public void DefaultUnion_RoundTrips()
    {
        RoundTrip(default(QueryResult<Dto>)).Value.ShouldBeNull();
    }

    [Test]
    public void UnionInsideAnObject_RoundTrips()
    {
        var wrapper = new Dictionary<string, UpdateResult> { ["a"] = Result.NoContent(), ["b"] = Result.NotFound() };

        var back = RoundTrip(wrapper);

        back["a"].Value.ShouldBeOfType<NoContentResult>();
        back["b"].Value.ShouldBeOfType<NotFoundResult>();
    }

    [Test]
    public void UnknownCase_Throws()
    {
        const string json = """{"case":"Some.Removed.Type","value":{}}""";

        Should.Throw<JsonException>(() => JsonSerializer.Deserialize<QueryResult<Dto>>(json, Options));
    }
}
