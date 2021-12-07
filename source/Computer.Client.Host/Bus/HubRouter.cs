using Computer.Client.Host.App;
using Computer.Client.Host.Controllers;
using Computer.Client.Host.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.Json;
using AppEvents = Computer.Client.Host.App.Events;

namespace Computer.Client.Host.Bus;

public class HubRouter : IEventHandler, IHubRouter
{
    private readonly IBus bus;
    private readonly IHubContext<BusHub, IBusHub> _busHub;
    private readonly ConcurrentDictionary<string, SubjectConfig> toBackendFromUiToBackend;

    private static object DirectConvert(Type type, JsonElement from)
    {
        return Convert.ChangeType(from, type);
    }

    private readonly ConcurrentDictionary<string, SubjectConfig> toUiFromBackendToUi = new ConcurrentDictionary<string, SubjectConfig>(new Dictionary<string, SubjectConfig>
    {
        {"subject name", new SubjectConfig(typeof(int), DirectConvert) },
        { AppEvents.GetConnectionResponse, new SubjectConfig(typeof(AppConnectionResponse), (type, from) =>
        {
            throw new NotImplementedException();
        }) },
        { AppEvents.CloseConnectionResponse, new SubjectConfig() },
    });
    
    private IEnumerable<IDisposable> _subscriptions = Enumerable.Empty<IDisposable>();
    
    public HubRouter(
        IBus bus,
        IHubContext<BusHub, IBusHub> busHub)
    {
        this.bus = bus;
        _busHub = busHub;

        toBackendFromUiToBackend = new ConcurrentDictionary<string, SubjectConfig>(new Dictionary<string, SubjectConfig>
        {
            {"subject name", new SubjectConfig(typeof(int), DirectConvert)},
            { AppEvents.GetConnection, new SubjectConfig(typeof(AppConnectionRequest), (type, from) =>
            {
                var o = JsonSerializer.Deserialize<AppConnectionRequest>(from, HostJsonContext.Default.AppConnectionRequest);
                return o;
            }) },
            { AppEvents.CloseConnection, new SubjectConfig(typeof(AppDisconnectRequest), (type, from) =>
            {
                var o = JsonSerializer.Deserialize<AppDisconnectRequest>(from, HostJsonContext.Default.AppDisconnectRequest);
                return o;
            }) },
        });
    }

    public void ReStartListening()
    {
        StopListening();
        var subs = new List<IDisposable>();
        foreach (var subject in toUiFromBackendToUi)
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
        foreach (var subscription in _subscriptions) {
            try
            {
                subscription.Dispose();
            }
            catch {
                //nothing, we just dont want to fail while disposing
            }            
        }
        _subscriptions = Enumerable.Empty<IDisposable>();
    }

    private async Task ConvertToHubEvent(BusEvent busEvent)
    {
        var @event = new EventForFrontEnd(busEvent.Subject, busEvent.EventId, busEvent.CorrelationId, busEvent.Param);
        await _busHub.Clients.All.EventToFrontEnd(@event);
    }

    public Task HandleBackendEvent(string subject, string eventId, string correlationId, JsonElement? eventObj = null)
    {
        if (toBackendFromUiToBackend.TryGetValue(subject, out SubjectConfig? config))
        {
            if (config.type is null)
            {
                bus.Publish(subject);
            }
            else
            {
                if (eventObj != null && config.ConvertFromHub != null)
                {
                    var obj = config.ConvertFromHub(config.type, (JsonElement)eventObj);
                    bus.Publish(subject, config.type, obj, eventId, correlationId);
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