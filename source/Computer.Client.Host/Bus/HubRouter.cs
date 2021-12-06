using Computer.Client.Host.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Computer.Client.Host.Bus;

public class HubRouter : IEventHandler, IHubRouter
{
    private readonly IBus bus;
    private readonly IHubContext<BusHub, IBusHub> _busHub;
    private readonly ConcurrentDictionary<string, SubjectConfig> subjectsFromUiToBackend = new ConcurrentDictionary<string, SubjectConfig>(new Dictionary<string, SubjectConfig>
    {
        {"subject name", new SubjectConfig(typeof(int)) },
    });
    private readonly ConcurrentDictionary<string, SubjectConfig> subjectsFromBackendToUi = new ConcurrentDictionary<string, SubjectConfig>(new Dictionary<string, SubjectConfig>
    {
        {"subject name", new SubjectConfig(typeof(int)) },
    });
    private IEnumerable<IDisposable>? _subscriptions = null;
    
    public HubRouter(
        IBus bus,
        IHubContext<BusHub, IBusHub> busHub)
    {
        this.bus = bus;
        _busHub = busHub;
    }

    public void ReStartListening()
    {
        StopListening();
        var subs = new List<IDisposable>();
        foreach (var subject in subjectsFromBackendToUi)
        {
            var subscription = subject.Value.type == null
                ? bus.Subscribe(subject.Key, e => ConvertToHubEvent(e))
                : bus.Subscribe(subject.Key, subject.Value.type, e => ConvertToHubEvent(e));

            subs.Add(subscription);
        }
        _subscriptions = subs;
    }
    public void StopListening()
    {
        if(_subscriptions == null)
        {
            return;
        }
        foreach (var subscription in _subscriptions) { 
            subscription.Dispose(); 
        }
        _subscriptions = null;
    }

    private void ConvertToHubEvent(BusEvent busEvent)
    {
        var @event = new EventForFrontEnd(busEvent.Subject, busEvent.EventId, busEvent.CorrelationId, busEvent.Param);
        _busHub.Clients.All.EventToFrontEnd(@event);
    }

    public Task HandleBackendEvent(EventForBackEnd @event)
    {
        if (subjectsFromUiToBackend.TryGetValue(@event.subject, out SubjectConfig? config))
        {
            if (config.type is null)
            {
                bus.Publish(@event.subject);
            }
            else
            {
                if (@event.eventObj != null)
                {
                    bus.Publish(@event.subject, config.type, @event.eventObj);
                }
            }
        }
        return Task.CompletedTask;
    }

    public void Test()
    {
        bus.Publish<string>("subject name", "it works");
    }
}
