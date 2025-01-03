using Microsoft.Extensions.DependencyInjection;

namespace CraftersCloud.Core;

[PublicAPI]
public static class ServiceScopeExtensions
{
    public static T Resolve<T>(this IServiceScope scope) where T : notnull =>
        scope.ServiceProvider.GetRequiredService<T>();

    public static T? ResolveOptional<T>(this IServiceScope scope) where T : notnull =>
        scope.ServiceProvider.GetService<T>();
}