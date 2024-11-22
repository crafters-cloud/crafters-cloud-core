using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CraftersCloud.Core.AspNetCore.MinimalApi;

[PublicAPI]
public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder builder) where T : class =>
        builder.AddEndpointFilter<RequestValidatorFilter<T>>();
}