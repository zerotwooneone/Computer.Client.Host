using Computer.Client.Host.Hubs;
using System.Collections.Concurrent;

namespace Computer.Client.Host.Bus;

public class HubRouter : IEventHandler
{
    private readonly IBus bus;
    private readonly ConcurrentDictionary<string, SubjectConfig> subjectsFromUiToBackend = new ConcurrentDictionary<string, SubjectConfig>(new Dictionary<string, SubjectConfig>
    {
        {"subject name", new SubjectConfig(typeof(int)) },
    });
    private readonly ConcurrentDictionary<string, SubjectConfig> subjectsFromBackendToUi = new ConcurrentDictionary<string, SubjectConfig>(new Dictionary<string, SubjectConfig>
    {
        {"subject name", new SubjectConfig(typeof(int)) },
    });
    
    public HubRouter(IBus bus)
    {
        this.bus = bus;
        
        var subs = new List<IDisposable>();
        foreach(var subject in subjectsFromBackendToUi)
        {
            var subscription = subject.Value.type == null
                ? bus.Subscribe(subject.Key, ()=>FireEvent(subject.Key))
                : bus.Subscribe(subject.Key, subject.Value.type, o=>FireParamEvent(subject.Key, o));

            subs.Add(subscription);
        }
    }

    private void FireEvent(string subject)
    {
        ToHubEvent?.Invoke(this, new BusToHubEvent(subject, "", ""));
    }

    private void FireParamEvent(string subject, object obj)
    {
        ToHubEvent?.Invoke(this, new BusToHubEvent(subject, "", "", obj));
    }

    public event EventHandler<BusToHubEvent> ToHubEvent;

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
