using Computer.Bus.Domain;
using Computer.Bus.Domain.Contracts;
using Computer.Client.Host.Domain.Config;

namespace Computer.Client.Host.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection("Bus");
        serviceCollection.Configure<BusConfig>(configurationSection);
        serviceCollection.AddSingleton<IBus, Computer.Bus.Domain.Bus>();
        serviceCollection.AddSingleton<Initializer>(new Initializer());
        serviceCollection.AddSingleton<IMapperFactory, MapperFactory>();
        serviceCollection.AddSingleton<DomainMapRegistrationService>();

        serviceCollection.AddHostedService<DomainStartupService>();

        return serviceCollection;
    }
}