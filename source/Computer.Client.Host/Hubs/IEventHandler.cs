namespace Computer.Client.Host.Hubs;

public interface IEventHandler
{
    
    Task HandleBackendEvent(EventForBackend @event);

    void Test();
}

public record BusToHubEvent(string subject, string eventId, string correlationId, object? eventObj = null);


