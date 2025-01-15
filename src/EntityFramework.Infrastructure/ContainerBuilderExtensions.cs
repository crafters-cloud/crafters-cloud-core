using System.Reflection;
using Autofac;
using CraftersCloud.Core.Data;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

[PublicAPI]
public static class ContainerBuilderExtensions
{
    public static void CoreRegisterRepositoryTypes(this ContainerBuilder builder, Assembly[] assemblies)
    {
        builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
            .As(typeof(IRepository<>))
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(EntityFrameworkRepository<,>))
            .As(typeof(IRepository<,>))
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(assemblies)
            .Where(
                type =>
                    ImplementsInterface(typeof(IRepository<>), type) ||
                    type.Name.EndsWith("Repository", StringComparison.InvariantCulture)
            ).AsImplementedInterfaces().InstancePerLifetimeScope();
    }

    private static bool ImplementsInterface(Type interfaceType, Type concreteType) =>
        concreteType.GetInterfaces().Any(
            t =>
                (interfaceType.IsGenericTypeDefinition && t.IsGenericType
                    ? t.GetGenericTypeDefinition()
                    : t) == interfaceType);
}