namespace Computer.Client.Host.Bus;

public interface IBus
{
    Task Publish(string subject, Type type, object obj);
    Task Publish(string subject);
    IDisposable Subscribe(string subject, Action callback);
    IDisposable Subscribe(string key, Type type, Action<object> callback);
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

    public static IDisposable Subscribe<T>(this IBus bus, string subject, Type type, Action<T> callback)
    {
        return bus.Subscribe(subject, type, obj =>
        {
            if (typeof(T).IsAssignableFrom(type))
            {
                var param = (T)obj;
                callback(param);
            }
        });
    }
}
