using Computer.Client.Host.App;
using Microsoft.AspNetCore.SignalR;

namespace Computer.Client.Host.Hubs;

public partial class BusHub : Hub<IBusHub>
{
    private readonly IAppConnectionRepo appConnectionRepo;
    /// <summary>
    /// this allows async sending
    /// </summary>
    private readonly IHubContext<BusHub, IBusHub> busHubContext;
    private readonly ILogger<BusHub> logger;
    private readonly IEventHandler eventHandler;

    public BusHub(IAppConnectionRepo appConnectionRepo,
        IHubContext<BusHub, IBusHub> busHubContext,
        ILogger<BusHub> logger,
        IEventHandler eventHandler)
    {
        this.appConnectionRepo = appConnectionRepo;
        this.busHubContext = busHubContext;
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
        //await busHubContext.Clients.Group(GetAppGroupName(appId, instanceId)).SendAsync($"ToAppUi:{appId}.{instanceId}", args);
    }

    public async Task SendEventToBackend(EventForBackEnd @event)
    {
        await eventHandler.HandleBackendEvent(@event);
    }
}


