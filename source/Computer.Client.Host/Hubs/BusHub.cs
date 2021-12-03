using Computer.Client.Host.App;
using Microsoft.AspNetCore.SignalR;

namespace Computer.Client.Host.Hubs;

public class BusHub : Hub
{
    private readonly IAppConnectionRepo appConnectionRepo;

    public BusHub(IAppConnectionRepo appConnectionRepo)
    {
        this.appConnectionRepo = appConnectionRepo;
    }
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var clientId = Context.ConnectionId;
        var deleted = await appConnectionRepo.DeleteAllByClientId(clientId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task<AppConnectionDetails> GetConnection(string? instanceId = null)
    {
        var innerId = string.IsNullOrEmpty(instanceId)
            ? Guid.NewGuid().ToString()
            : instanceId;
        var clientId = Context.ConnectionId;
        return await appConnectionRepo.GetOrCreate(clientId, innerId);
    }
}
