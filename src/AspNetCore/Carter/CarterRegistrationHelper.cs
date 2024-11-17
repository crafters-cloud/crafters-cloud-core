using System.Reflection;
using Carter;
using JetBrains.Annotations;

namespace CraftersCloud.Core.AspNetCore.Carter;

[PublicAPI]
public static class CarterRegistrationHelper
{
    public static Type[] FindCarterModules(Assembly[] assemblies)
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