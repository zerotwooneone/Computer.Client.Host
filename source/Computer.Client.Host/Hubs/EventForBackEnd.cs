namespace Computer.Client.Host.Hubs;

public record EventForBackEnd(string subject, string eventId, string correlationId, object? eventObj = null);


