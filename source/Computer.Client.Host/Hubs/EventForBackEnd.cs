namespace Computer.Client.Host.Hubs;

public record EventForBackend(string subject, string eventId, string correlationId, object? eventObj = null);


