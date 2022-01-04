using RabbitMQ.Client;
using IConnectionFactory = Computer.Bus.RabbitMq.Contracts.IConnectionFactory;

namespace Computer.Client.Host.ExternalBus;

public class SingletonConnectionFactory : IConnectionFactory
{
    private static readonly Lazy<ConnectionFactory> ConnectionFactory = new(() => new ConnectionFactory()
        { HostName = "localhost", DispatchConsumersAsync = true });

    private static readonly Lazy<IConnection> Connection =
        new(() => ConnectionFactory.Value.CreateConnection());

    public Task<IConnection> GetConnection(string connectionId, string? subjectId = null)
    {
        return Task.FromResult(Connection.Value);
    }
}