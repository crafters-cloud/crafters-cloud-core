using System.Reflection;
using Carter;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.Core.AspNetCore.Carter;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static void AddCoreCarter(this IServiceCollection services, Assembly[] assembliesWithCarterModules) =>
        services.AddCarter(configurator: c =>
        {
            var carterModules = FindCarterModules(assembliesWithCarterModules);
            c.WithModules(carterModules);
        });

    private static Type[] FindCarterModules(Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        var modules = types
            .Where(t =>
                !t.IsAbstract &&
                typeof(ICarterModule).IsAssignableFrom(t) &&
                t != typeof(ICarterModule)
                && t.IsPublic
            ).ToArray();
        return modules;
    }
}