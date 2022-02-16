using System.Reactive.Linq;
using Computer.Client.Domain.Contracts.App;
using Computer.Client.Domain.Contracts.Bus;
using Computer.Client.Domain.Contracts.Model;
using Computer.Domain.Bus.Reactive.Contracts;
using Computer.Domain.Bus.Reactive.Contracts.Model;

namespace Computer.Client.Domain.App;

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
                .SelectMany(e => Observable.FromAsync(async _ => await OnConnectionRequest(e).ConfigureAwait(false)))
                .Subscribe(),
            // bus.Subscribe<AppDisconnectRequest>(Events.CloseConnection)
            //     .SelectMany(e => Observable.FromAsync(async _ => await OnDisconnectRequest(e)))
            //     .Subscribe(),
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

    // private Task OnDisconnectRequest(IBusEvent<AppDisconnectRequest> busEvent)
    // {
    //     //todo: fill this in later
    //     return Task.CompletedTask;
    // }

    private async Task OnConnectionRequest(IBusEvent<AppConnectionRequest> busEvent)
    {
        if (busEvent.Param == null)
        {
            throw new InvalidOperationException("Connection Param was null");
        }
        await Task.Delay(100).ConfigureAwait(false); //simulate some work
        //todo: tell BusHub to connect the application so we can skip the bus
        await bus.Publish(Events.GetConnectionResponse, new AppConnectionResponse(),
            correlationId: busEvent.CorrelationId).ConfigureAwait(false);
    }
}