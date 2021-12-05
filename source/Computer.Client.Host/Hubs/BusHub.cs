using Computer.Client.Host.App;
using Microsoft.AspNetCore.SignalR;

namespace Computer.Client.Host.Hubs;

public class BusHub : Hub
{
    private readonly IAppConnectionRepo appConnectionRepo;
    /// <summary>
    /// todo: remove this
    /// </summary>
    private readonly IHubContext<BusHub> busHubContext;
    private readonly ILogger<BusHub> logger;
    private readonly IEventHandler eventHandler;

    public BusHub(IAppConnectionRepo appConnectionRepo,
        IHubContext<BusHub> busHubContext,
        ILogger<BusHub> logger,
        IEventHandler eventHandler)
    {
        this.appConnectionRepo = appConnectionRepo;
        this.busHubContext = busHubContext;
        this.logger = logger;
        this.eventHandler = eventHandler;
        this.eventHandler.ToHubEvent += OnParameterEvent;
    }

    private async void OnParameterEvent(object? sender, BusToHubEvent e)
    {
        await SendEventToFrontEnd(e.subject, e.eventId, e.eventObj, e.correlationId);
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override Task OnConnectedAsync()
    {
        logger.LogDebug($"Connected {Context.ConnectionId}");
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var clientId = Context.ConnectionId;
        var deleted = await appConnectionRepo.DeleteAllByClientId(clientId);
        logger.LogDebug($"Disconnected {clientId} deletedApp:{deleted}");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task<AppConnectionDetails?> GetConnection(string appId, string? instanceId = null)
    {
        if (string.IsNullOrWhiteSpace(appId))
        {
            return null;
        }
        var innerId = string.IsNullOrEmpty(instanceId)
            ? Guid.NewGuid().ToString()
            : instanceId;
        return await InnerGetConnection(appId, innerId);
    }

    private async Task<AppConnectionDetails> InnerGetConnection(string appId, string instanceId)
    {
        var clientId = Context.ConnectionId;
        await Groups.AddToGroupAsync(Context.ConnectionId, GetAppGroupName(appId, instanceId));

        var t = new Thread(async () =>
        {
            Thread.Sleep(2000);
            await SendToAppUi(appId, instanceId, new { test = "something" });
        });
        t.Start();

        eventHandler.Test();

        return await appConnectionRepo.GetOrCreate(appId, clientId, instanceId);
    }

    public async Task CloseConnection(string appId, string instanceId)
    {
        if (string.IsNullOrWhiteSpace(appId))
        {
            throw new ArgumentException($"'{nameof(appId)}' cannot be null or whitespace.", nameof(appId));
        }

        if (string.IsNullOrWhiteSpace(instanceId))
        {
            throw new ArgumentException($"'{nameof(instanceId)}' cannot be null or whitespace.", nameof(instanceId));
        }
        var clientId = Context.ConnectionId;
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetAppGroupName(appId, instanceId));
        var deleted = await appConnectionRepo.TryDelete(appId, clientId, instanceId);
    }

    private static string GetAppGroupName(string appId, string instanceId)
    {
        return $"{appId}.{instanceId}";
    }

    public async Task SendToAppUi(string appId, string instanceId, params object[] args)
    {
        if (string.IsNullOrWhiteSpace(appId))
        {
            throw new ArgumentException($"'{nameof(appId)}' cannot be null or whitespace.", nameof(appId));
        }

        if (string.IsNullOrWhiteSpace(instanceId))
        {
            throw new ArgumentException($"'{nameof(instanceId)}' cannot be null or whitespace.", nameof(instanceId));
        }
        await busHubContext.Clients.Group(GetAppGroupName(appId, instanceId)).SendAsync($"ToAppUi:{appId}.{instanceId}", args);
    }

    public async Task SendEventToFrontEnd(string subject, string? eventId = null, object? eventObj = null, string? correlationId = null)
    {
        var internalEventId = string.IsNullOrWhiteSpace(eventId)
            ? Guid.NewGuid().ToString()
            : eventId;
        var internalCorrelationId = string.IsNullOrWhiteSpace(correlationId)
            ? Guid.NewGuid().ToString()
            : correlationId;
        await InternalSendEventToFrontEnd(subject, eventObj, internalEventId, internalCorrelationId);
    }

    private async Task InternalSendEventToFrontEnd(string subject, object? eventObj, string eventId, string correlationId)
    {
        var @event = new EventForFrontEnd(subject, eventId, correlationId, eventObj);
        await busHubContext.Clients.All.SendAsync("EventToFrontEnd", @event);
    }

    public async Task SendEventToBackend(EventForBackEnd @event)
    {
        await eventHandler.HandleBackendEvent(@event);
    }

    protected record EventForFrontEnd(string subject, string eventId, string correlationId, object? eventObj = null);
}


