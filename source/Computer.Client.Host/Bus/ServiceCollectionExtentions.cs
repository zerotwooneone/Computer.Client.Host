using System.Reactive.Concurrency;
using Computer.Client.Host.Hubs;
using Computer.Domain.Bus.Contracts;
using Computer.Domain.Bus.Reactive;
using Computer.Domain.Bus.Reactive.Contracts;

namespace Computer.Client.Host.Bus;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddBus(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<IScheduler>(new TaskPoolScheduler(new TaskFactory()))
            .AddSingleton<IReactiveBus, ReactiveBus>()
            .AddSingleton<IRequestService, ReactiveRequestService>()
            .AddSingleton<ExternalRouter>()
            .AddSingleton<HubRouter>() //this ensures only one instance, even though we implement multiple interfaces
            .AddSingleton<IEventHandler>(x => x.GetRequiredService<HubRouter>())
            .AddSingleton<IHubRouter>(x => x.GetRequiredService<HubRouter>())
            .AddHostedService<BusInitialization>();

    }
}