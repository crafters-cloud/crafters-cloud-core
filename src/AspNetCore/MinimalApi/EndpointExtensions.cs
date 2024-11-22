using System.Reflection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CraftersCloud.Core.AspNetCore.MinimalApi;

[PublicAPI]
public static class EndpointExtensions
{
    /// <summary>
    ///  Add all the endpoints from the given assembly.
    /// </summary>
    /// <param name="services">Services</param>
    /// <param name="assemblies">Assemblies containing implementors of <see cref="IEndpoint"/> interface</param>
    /// <returns></returns>
    public static IServiceCollection AddCoreEndpoints(this IServiceCollection services, params Assembly[] assemblies)
    {
        var serviceDescriptors = assemblies.SelectMany(a => a.DefinedTypes)
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    /// <summary>
    /// Maps all the endpoints to the application.
    /// </summary>
    /// <param name="application">The application</param>
    /// <param name="routeGroupBuilder">Route group builder</param>
    /// <returns>Application</returns>
    public static WebApplication MapCoreEndpoints(
        this WebApplication application,
        RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = application.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? application : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return application;
    }
}