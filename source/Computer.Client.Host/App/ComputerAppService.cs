using Computer.Client.Host.Bus;

namespace Computer.Client.Host.App;

public class ComputerAppService : IComputerAppService
{
    private readonly IBus bus;
    private readonly List<IDisposable> subscriptions = new List<IDisposable>();

    public ComputerAppService(IBus bus)
    {
        this.bus = bus;
    }
    public Task ReStartListening()
    {
        subscriptions.Add(
            bus.Subscribe<AppConnectionRequest>(Events.GetConnection, OnConnectionRequest)
        );
        subscriptions.Add(
            bus.Subscribe<AppDisconnectRequest>(Events.CloseConnection, OnDisconnectRequest)
        );
        return Task.CompletedTask;
    }

    public void StopListening()
    {
        foreach (var subscription in subscriptions)
        {
            try
            {
                subscription.Dispose();
            }
            catch
            {
                //nothing
            }
        }
        subscriptions.Clear();
    }

    private Task OnDisconnectRequest(BusEvent<AppDisconnectRequest> busEvent)
    {
        //todo: fill this in later
        return Task.CompletedTask;
    }

    private async Task OnConnectionRequest(BusEvent<AppConnectionRequest> busEvent)
    {
        var instanceId = busEvent.Param.instanceId ?? Guid.NewGuid().ToString();
        await bus.Publish(Events.GetConnectionResponse, new AppConnectionResponse(instanceId), correlationId: busEvent.CorrelationId);
    }
    
}

public static class Events
{
    public const string GetConnection = "GetConnection";
    public const string CloseConnection = "CloseConnection";
    public const string GetConnectionResponse = "GetConnectionResponse";
    public const string CloseConnectionResponse = "CloseConnectionResponse";
}

public record AppConnectionRequest(string? instanceId, string appId);
public record AppDisconnectRequest(string instanceId, string appId);
public record AppConnectionResponse(string instanceId);
