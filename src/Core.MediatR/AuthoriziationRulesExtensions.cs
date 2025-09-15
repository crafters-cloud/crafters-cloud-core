using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.Core.MediatR;

public static class AuthoriziationRulesExtensions
{
    public static void AddRules(this IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
    {
        assembliesToScan = (assembliesToScan as Assembly[] ?? assembliesToScan).Distinct().ToArray();

        var authorizationType = typeof(IAuthorizationRule<>);
        foreach (var assembly in assembliesToScan)
        {
            assembly.GetTypesAssignableTo(authorizationType).ForEach(type =>
            {
                foreach (var implementedInterface in type.ImplementedInterfaces)
                {
                    services.AddTransient(implementedInterface, type);
                }
            });
        }
    }

    private static List<TypeInfo> GetTypesAssignableTo(this Assembly assembly, Type compareType)
    {
        var typeInfoList = assembly.DefinedTypes.Where(x =>
            x is {IsClass: true, IsAbstract: false} && x != compareType &&
            Enumerable.Any(x.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == compareType)).ToList();

        return typeInfoList;
    }
}