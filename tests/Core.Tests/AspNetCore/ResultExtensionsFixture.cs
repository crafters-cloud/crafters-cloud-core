using CraftersCloud.Core.AspNetCore.MinimalApi;
using CraftersCloud.Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using ValidationProblemDetails = Microsoft.AspNetCore.Mvc.ValidationProblemDetails;

namespace CraftersCloud.Core.Tests.AspNetCore;

[Category("unit")]
public class ResultExtensionsFixture
{
    public sealed record Dto(int Id);

    private readonly DefaultHttpContext _httpContext = new() { Request = { Path = "/dtos" } };

    [Test]
    public void QueryResult_MapsToOkOrNotFound()
    {
        QueryResult<Dto> found = new Dto(1);
        QueryResult<Dto> notFound = Result.NotFound();

        found.ToMinimalApiResult(x => x.Id).Result.ShouldBeOfType<Ok<int>>().Value.ShouldBe(1);
        notFound.ToMinimalApiResult().Result.ShouldBeOfType<NotFound>();
    }

    [Test]
    public void CreateResult_MapsToCreatedOrBadRequest()
    {
        CreateResult<Dto> created = Result.Created(new Dto(1));
        CreateResult<Dto> badRequest = Result.BadRequest("name", "Required");

        var createdResult = created.ToMinimalApiResult(_httpContext, x => $"/dtos/{x.Id}").Result.ShouldBeOfType<Created<Dto>>();
        createdResult.Location.ShouldBe("/dtos/1");

        var problem = badRequest.ToMinimalApiResult(_httpContext, x => $"/dtos/{x.Id}").Result
            .ShouldBeOfType<BadRequest<ValidationProblemDetails>>().Value.ShouldNotBeNull();
        problem.Errors["name"].ShouldBe(["Required"]);
        problem.Instance.ShouldBe("/dtos");
    }

    [Test]
    public void UpdateResult_MapsToNoContentNotFoundOrBadRequest()
    {
        ((UpdateResult) Result.NoContent()).ToMinimalApiResult(_httpContext).Result.ShouldBeOfType<NoContent>();
        ((UpdateResult) Result.NotFound()).ToMinimalApiResult(_httpContext).Result.ShouldBeOfType<NotFound>();
        ((UpdateResult) Result.BadRequest("name", "Required")).ToMinimalApiResult(_httpContext).Result
            .ShouldBeOfType<BadRequest<ValidationProblemDetails>>();
    }

    [Test]
    public void DefaultUnion_Throws()
    {
        Should.Throw<InvalidOperationException>(() => default(UpdateResult).ToMinimalApiResult(_httpContext));
    }
}
