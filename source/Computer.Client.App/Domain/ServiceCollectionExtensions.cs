using Computer.Bus.Domain;
using Computer.Bus.Domain.Contracts;
using Computer.Client.App.Domain.Config;
using Computer.Client.Domain.App;
using Computer.Client.Domain.App.ToDoList;
using Computer.Client.Domain.Contracts.App;
using Computer.Client.Domain.Contracts.App.ToDoList;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Computer.Client.App.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var section = configuration.GetSection("Bus");
        serviceCollection.Configure<BusConfig>(section);
        serviceCollection.AddSingleton<IBus, Computer.Bus.Domain.Bus>();
        serviceCollection.AddSingleton<Initializer>(new Initializer());
        serviceCollection.AddSingleton<IMapperFactory, MapperFactory>();
        serviceCollection.AddSingleton<DomainMapRegistrationService>();
        
        serviceCollection.AddSingleton<IListService, ListService>();
        serviceCollection.AddSingleton<IAppService, DummyAppService>();
        serviceCollection.AddSingleton<IComputerAppService, ComputerAppService>();

        serviceCollection.AddHostedService<DomainStartupService>();

        return serviceCollection;
    }
}