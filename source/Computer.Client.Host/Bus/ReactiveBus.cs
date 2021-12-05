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
    public Task Publish(string subject, Type type, object obj)
    {
        var busEvent = new BusEvent(subject, type, obj);
        _bus.OnNext(busEvent);
        return Task.CompletedTask;
    }

    public Task Publish(string subject)
    {
        var busEvent = new BusEvent(subject);
        _bus.OnNext(busEvent);
        return Task.CompletedTask;
    }

    public IDisposable Subscribe(string subject, Type type, Action<object> callback)
    {
        return _bus
            .ObserveOn(scheduler)
            .Subscribe(e =>
            {
                if (e.param != null)
                {
                    callback(e.param);
                }
            });
    }

    public IDisposable Subscribe(string subject, Action callback)
    {
        return _bus
            .ObserveOn(scheduler)
            .Subscribe(e =>
            {
                callback();
            });
    }

    private record BusEvent(string subject, Type? type = null, object? param = null);
}
