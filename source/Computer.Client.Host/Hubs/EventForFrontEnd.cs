namespace Computer.Client.Host.Hubs;

public record EventForFrontEnd(string subject, string eventId, string correlationId, object? eventObj = null);