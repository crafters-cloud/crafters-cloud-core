namespace CraftersCloud.Core.AspNetCore.Tests.Http;

public static class AssertionExtensions
{
    public static HttpResponseAssertions Should(this HttpResponseMessage actualValue) => new(actualValue);
}