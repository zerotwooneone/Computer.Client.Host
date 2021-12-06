namespace Computer.Client.Host.Hubs;

public interface IEventHandler
{
    
    Task HandleBackendEvent(EventForBackEnd @event);

    event EventHandler<BusToHubEvent> ToHubEvent;

    void Test();
}

public record BusToHubEvent(string subject, string eventId, string correlationId, object? eventObj = null);


