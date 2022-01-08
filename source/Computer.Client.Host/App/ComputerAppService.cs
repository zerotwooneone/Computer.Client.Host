using System.Reactive.Linq;
using Computer.Domain.Bus.Reactive.Contracts;
using Computer.Domain.Bus.Reactive.Contracts.Model;

namespace Computer.Client.Host.App;

public class ComputerAppService : IComputerAppService
{
    private readonly IReactiveBus bus;
    private readonly List<IDisposable> subscriptions = new();

    public ComputerAppService(IReactiveBus bus)
    {
        this.bus = bus;
    }

    public Task ReStartListening()
    {
        subscriptions.AddRange(new []
        {
            bus.Subscribe<AppConnectionRequest>(Events.GetConnection)
                .SelectMany(e => Observable.FromAsync(async _ => await OnConnectionRequest(e)))
                .Subscribe(),
            bus.Subscribe<AppDisconnectRequest>(Events.CloseConnection)
                .SelectMany(e => Observable.FromAsync(async _ => await OnDisconnectRequest(e)))
                .Subscribe(),
        });
        return Task.CompletedTask;
    }

    public void StopListening()
    {
        foreach (var subscription in subscriptions)
            try
            {
                subscription.Dispose();
            }
            catch
            {
                //nothing
            }

        subscriptions.Clear();
    }

    private Task OnDisconnectRequest(IBusEvent<AppDisconnectRequest> busEvent)
    {
        //todo: fill this in later
        return Task.CompletedTask;
    }

    private async Task OnConnectionRequest(IBusEvent<AppConnectionRequest> busEvent)
    {
        if (busEvent.Param == null)
        {
            throw new InvalidOperationException("Connection Param was null");
        }
        var instanceId = busEvent.Param.instanceId ?? Guid.NewGuid().ToString();
        await Task.Delay(100); //simulate some work
        //todo: tell BusHub to connect the application so we can skip the bus
        await bus.Publish(Events.GetConnectionResponse, new AppConnectionResponse(instanceId),
            correlationId: busEvent.CorrelationId);
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