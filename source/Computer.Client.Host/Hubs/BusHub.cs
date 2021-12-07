using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Computer.Client.Host.Hubs;

public partial class BusHub : Hub<IBusHub>
{
    private readonly ILogger<BusHub> logger;
    private readonly IEventHandler eventHandler;

    public BusHub(
        ILogger<BusHub> logger,
        IEventHandler eventHandler)
    {
        this.logger = logger;
        this.eventHandler = eventHandler;
    }

    public override Task OnConnectedAsync()
    {
        logger.LogDebug($"Connected {Context.ConnectionId}");
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var clientId = Context.ConnectionId;
        logger.LogDebug($"Disconnected {clientId}");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendEventToBackend(string? subject, string? eventId, string? correlationId, object? eventObj = null)
    {
        if (string.IsNullOrWhiteSpace(subject) ||
            string.IsNullOrWhiteSpace(eventId) ||
            string.IsNullOrWhiteSpace(correlationId))
        {
            return;
        }
        
        await eventHandler.HandleBackendEvent(subject, eventId, correlationId, (JsonElement?)eventObj);
    }
}


