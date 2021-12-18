using System.Reflection;
using Autofac;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.EntityFramework.Infrastructure.Security;
using CraftersCloud.Core.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Module = Autofac.Module;

namespace CraftersCloud.Core.EntityFramework.Infrastructure;

public class EntityFrameworkModule<TContext> : Module where TContext : DbContext
{
    protected EntityFrameworkModule(
        string connectionStringName,
        Action<DbContextOptionsBuilder> configureDbContextOptions,
        Action<SqlServerDbContextOptionsBuilder> configureSqlOptions,
        bool registerMigrationsAssembly,
        string migrationAssemblyName,
        params Assembly[] customRepositoriesAssemblies)
    {
        ConnectionStringName = connectionStringName;
        ConfigureDbContextOptions = configureDbContextOptions;
        ConfigureSqlOptions = configureSqlOptions;
        RegisterMigrationsAssembly = registerMigrationsAssembly;
        MigrationAssemblyNameName = migrationAssemblyName;
        CustomRepositoriesAssemblies = customRepositoriesAssemblies;
    }

    private bool RegisterMigrationsAssembly { get; }

    private string MigrationAssemblyNameName { get; }

    private Assembly[] CustomRepositoriesAssemblies { get; }

    private string ConnectionStringName { get; }

    private Action<DbContextOptionsBuilder> ConfigureDbContextOptions { get; }

    private Action<SqlServerDbContextOptionsBuilder> ConfigureSqlOptions { get; }

    protected override void Load(ContainerBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.RegisterGeneric(typeof(EntityFrameworkRepository<>))
            .As(typeof(IRepository<>))
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(EntityFrameworkRepository<,>))
            .As(typeof(IRepository<,>))
            .InstancePerLifetimeScope();

        builder.RegisterAssemblyTypes(CustomRepositoriesAssemblies)
            .Where(
                type =>
                    ImplementsInterface(typeof(IRepository<>), type) ||
                    type.Name.EndsWith("Repository", StringComparison.InvariantCulture)
            ).AsImplementedInterfaces().InstancePerLifetimeScope();

        builder.Register(CreateDbContextOptions).As<DbContextOptions>().SingleInstance();

        // needs to be registered both as self and as DbContext or the tests might not work as expected
        builder.RegisterType<TContext>().AsSelf().As<DbContext>().InstancePerLifetimeScope();
        builder.RegisterType<DbContextUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

        builder.RegisterType<DbContextAccessTokenProvider>().As<IDbContextAccessTokenProvider>()
            .InstancePerLifetimeScope();
    }

    private static bool ImplementsInterface(Type interfaceType, Type concreteType) =>
        concreteType.GetInterfaces().Any(
            t =>
                (interfaceType.IsGenericTypeDefinition && t.IsGenericType
                    ? t.GetGenericTypeDefinition()
                    : t) == interfaceType);

    private DbContextOptions CreateDbContextOptions(IComponentContext container)
    {
        var loggerFactory = container.Resolve<ILoggerFactory>();
        var configuration = container.Resolve<IConfiguration>();
        var dbContextSettings = container.Resolve<DbContextSettings>();

        var optionsBuilder = new DbContextOptionsBuilder();

        optionsBuilder
            .UseLoggerFactory(loggerFactory)
            .EnableSensitiveDataLogging(dbContextSettings.SensitiveDataLoggingEnabled);

        ConfigureDbContextOptions(optionsBuilder);

        optionsBuilder.UseSqlServer(configuration.GetConnectionString(ConnectionStringName),
                sqlOptions =>
                {
                    SetupSqlOptions(sqlOptions, dbContextSettings);
                    ConfigureSqlOptions(sqlOptions);
                })
            .ConfigureWarnings(x => x.Default(WarningBehavior.Log))
            .ConfigureWarnings(x => x.Log(RelationalEventId.MultipleCollectionIncludeWarning));

        return optionsBuilder.Options;
    }

    private SqlServerDbContextOptionsBuilder SetupSqlOptions(SqlServerDbContextOptionsBuilder sqlOptions,
        DbContextSettings dbContextSettings)
    {
        //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
        sqlOptions = sqlOptions.EnableRetryOnFailure(
            dbContextSettings.ConnectionResiliencyMaxRetryCount,
            dbContextSettings.ConnectionResiliencyMaxRetryDelay,
            null);

        // prefer splitting queries by default (avoids manually splitting queries throughout the app)
        sqlOptions = sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

        if (RegisterMigrationsAssembly)
        {
            sqlOptions = sqlOptions.MigrationsAssembly(MigrationAssemblyNameName);
        }

        return sqlOptions;
    }
}