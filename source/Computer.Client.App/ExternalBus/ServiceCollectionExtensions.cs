using Computer.Bus.Contracts;
using Computer.Bus.ProtobuffNet;
using Computer.Bus.RabbitMq;
using Computer.Bus.RabbitMq.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Computer.Client.App.ExternalBus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalBus(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISerializer, ProtoSerializer>();
        serviceCollection.AddSingleton<IConnectionFactory, SingletonConnectionFactory>();
        serviceCollection.AddSingleton<IBusClient>(serviceProvider =>
        {
            var clientFactory = new ClientFactory();
            var serializer = serviceProvider.GetService<ISerializer>() ?? throw new InvalidOperationException();
            var connectionFactory = serviceProvider.GetService<IConnectionFactory>() ?? throw new InvalidOperationException();
            return clientFactory.Create(serializer, connectionFactory);
        });
        serviceCollection.AddSingleton<IRequestService, Computer.Bus.RabbitMq.Client.RequestService>();
        
        return serviceCollection;
    }
}