using Autofac;
using System.Reflection;

namespace CraftersCloud.Core.EventBus;

public static class EventBusStartupExtensions
{
    public static void AppAddServiceBus(this ContainerBuilder containerBuilder, EventBusSettings settings,
        Assembly[] assembliesWithIntegrationEvents) =>
        containerBuilder.RegisterModule(new EventBusModule
        {
            AssembliesWithIntegrationEvents = assembliesWithIntegrationEvents,
            EventBusSettings = settings
        });
}