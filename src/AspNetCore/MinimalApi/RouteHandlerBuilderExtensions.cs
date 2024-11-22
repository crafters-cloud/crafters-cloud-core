using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CraftersCloud.Core.AspNetCore.MinimalApi;

[PublicAPI]
public static class RouteHandlerBuilderExtensions
{
    /// <summary>
    /// Adds a request validator filter to the route handler.
    /// </summary>
    /// <param name="builder">The builder</param>
    /// <typeparam name="T">Type of class to validate, the class that might have registered validator</typeparam>
    /// <returns>The builder</returns>
    public static RouteHandlerBuilder Validate<T>(this RouteHandlerBuilder builder) where T : class =>
        builder.AddEndpointFilter<RequestValidatorFilter<T>>();
}