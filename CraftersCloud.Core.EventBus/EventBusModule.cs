using Autofac;
using CraftersCloud.Core.EventBus.ServiceBus;
using CraftersCloud.Core.EventBus.ServiceBus.Rules;
using CraftersCloud.Core.IntegrationEvents;
using Microsoft.Extensions.Logging;
using System.Reflection;
using CraftersCloud.Core.Helpers;
using Module = Autofac.Module;

namespace CraftersCloud.Core.EventBus;

public class EventBusModule : Module
{
    public Assembly[] AssembliesWithIntegrationEvents { get; set; } = Array.Empty<Assembly>();
    public EventBusSettings EventBusSettings { get; set; } = new();

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(CreateEventBus).As<IEventBus>().SingleInstance();
        builder.Register(CreatePersisterConnection).As<IServiceBusPersisterConnection>().SingleInstance();
        builder.Register(CreateRulesProvider).As<IServiceBusRulesProvider>().SingleInstance();
        builder.RegisterType<InMemoryEventBusSubscriptionsManager>().As<IEventBusSubscriptionsManager>()
            .SingleInstance();
        RegisterIntegrationEventHandlers(builder);
    }

    private IEventBus CreateEventBus(IComponentContext container)
    {
        if (!EventBusSettings.Enabled)
        {
            return new NullEventBus();
        }

        var serviceBusPersisterConnection = container.Resolve<IServiceBusPersisterConnection>();
        var iLifetimeScope = container.Resolve<ILifetimeScope>();
        var logger = container.Resolve<ILogger<EventBusServiceBus>>();
        var eventBusSubscriptionsManager = container.Resolve<IEventBusSubscriptionsManager>();
        var rulesProvider = container.Resolve<IServiceBusRulesProvider>();

        return new EventBusServiceBus(serviceBusPersisterConnection, logger,
            eventBusSubscriptionsManager, EventBusSettings.SubscriptionClientName,
            iLifetimeScope, rulesProvider);
    }


    private IServiceBusPersisterConnection CreatePersisterConnection(IComponentContext container)
    {
        var serviceBusConnectionString = EventBusSettings.ConnectionString;
        var connectionPersister = new DefaultServiceBusPersisterConnection(serviceBusConnectionString);
        return connectionPersister;
    }

    private IServiceBusRulesProvider CreateRulesProvider(IComponentContext container) =>
        new DefaultRulesProvider(EventBusSettings.SubscriptionClientName);

    private void RegisterIntegrationEventHandlers(ContainerBuilder builder) =>
        builder.RegisterAssemblyTypes(AssembliesWithIntegrationEvents)
            .Where(
                type =>
                    type.ImplementsInterface(typeof(IIntegrationEventHandler<>))
            ).AsSelf().InstancePerDependency();
}