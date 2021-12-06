namespace Computer.Client.Host.Bus;

public interface IBus
{
    Task Publish(string subject, Type type, object obj, string? eventId = null, string? correlationId = null);
    Task Publish(string subject, string? eventId = null, string? correlationId = null);
    IDisposable Subscribe(string subject, Action<BusEvent> callback);
    IDisposable Subscribe(string key, Type type, Action<BusEvent> callback);
}

public static class BusExtensions
{
    public static Task Publish<T>(this IBus bus, string subject, T obj)
    {
        if(obj == null)
        {
            return Task.CompletedTask;
        }
        return bus.Publish(subject, typeof(T), obj);
    }

    public static IDisposable Subscribe<T>(this IBus bus, string subject, Type type, Action<BusEvent<T>> callback, string? eventId = null, string? correlationId = null)
    {
        return bus.Subscribe(subject, type, obj =>
        {
            if (typeof(T).IsAssignableFrom(type) && 
                obj.Param !=null)
            {
                var param = (T)obj.Param;
                var @event = new BusEvent<T>(subject, type, param, eventId, correlationId);
                callback(@event);
            }
        });
    }
}
