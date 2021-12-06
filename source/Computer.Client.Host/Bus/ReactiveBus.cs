using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Computer.Client.Host.Bus;

public class ReactiveBus : IBus
{
    private readonly IScheduler scheduler;
    private readonly Subject<BusEvent> _bus;
    public ReactiveBus(IScheduler scheduler)
    {
        this.scheduler = scheduler;
        _bus = new Subject<BusEvent>();
    }
    public Task Publish(string subject, Type type, object obj, string? eventId = null, string? correlationId = null)
    {
        var busEvent = new BusEvent(subject, type, obj, eventId, correlationId);
        _bus.OnNext(busEvent);
        return Task.CompletedTask;
    }

    public Task Publish(string subject, string? eventId = null, string? correlationId = null)
    {
        var busEvent = new BusEvent(subject, eventId: eventId, correlationId: correlationId);
        _bus.OnNext(busEvent);
        return Task.CompletedTask;
    }

    public IDisposable Subscribe(string subject, Type type, Action<BusEvent> callback)
    {
        return _bus
            .ObserveOn(scheduler)
            .Subscribe(callback);
    }

    public IDisposable Subscribe(string subject, Action<BusEvent> callback)
    {
        return _bus
            .ObserveOn(scheduler)
            .Subscribe(callback);
    }
}
