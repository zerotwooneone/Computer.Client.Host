using System.Text.Json;

namespace Computer.Client.App.Hubs;

public interface IEventHandler
{
    Task HandleBackendEvent(string subject, string eventId, string correlationId, JsonElement? eventObj = null);

    void Test();
}

public record BusToHubEvent(string subject, string eventId, string correlationId, object? eventObj = null);