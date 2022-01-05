namespace Computer.Client.Host.Bus;

public interface IBus
{
    Task Publish(string subject, Type type, object obj, string? eventId = null, string? correlationId = null);
    Task Publish(string subject, string? eventId = null, string? correlationId = null);
    IDisposable Subscribe(string subject, Func<BusEvent, Task> callback);
    IDisposable Subscribe(string key, Type type, Func<BusEvent, Task> callback);
}

public static class BusExtensions
{
    public static Task Publish<T>(
        this IBus bus,
        string subject,
        T obj,
        string? eventId = null,
        string? correlationId = null)
    {
        if (obj == null) return Task.CompletedTask;
        return bus.Publish(subject, typeof(T), obj, eventId, correlationId);
    }

    public static IDisposable Subscribe<T>(this IBus bus, string subject, Func<BusEvent<T>, Task> callback)
    {
        var type = typeof(T);
        return bus.Subscribe(subject, type, busEvent =>
        {
            if (!typeof(T).IsAssignableFrom(type) ||
                busEvent.Param == null)
                return Task.CompletedTask;
            var param = (T)busEvent.Param;
            var @event = new BusEvent<T>(subject, type, param, busEvent.EventId, busEvent.CorrelationId);
            return callback(@event);
        });
    }

    public static IDisposable Subscribe<T>(this IBus bus, string subject, Action<BusEvent<T>> callback)
    {
        var type = typeof(T);
        return bus.Subscribe(subject, type, busEvent =>
        {
            if (!typeof(T).IsAssignableFrom(type) ||
                busEvent.Param == null)
                return Task.CompletedTask;
            var param = (T)busEvent.Param;
            var @event = new BusEvent<T>(subject, type, param, busEvent.EventId, busEvent.CorrelationId);
            callback(@event);
            return Task.CompletedTask;
        });
    }
}