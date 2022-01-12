namespace Computer.Client.App.Hubs;

public record EventForFrontEnd(string subject, string eventId, string correlationId, object? eventObj = null);