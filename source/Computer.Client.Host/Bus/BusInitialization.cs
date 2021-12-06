namespace Computer.Client.Host.Bus;

public class BusInitialization : IHostedService
{
    private readonly IHubRouter _hubRouter;

    public BusInitialization(IHubRouter hubRouter)
    {
        this._hubRouter = hubRouter;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _hubRouter.ReStartListening();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _hubRouter.StopListening();
        return Task.CompletedTask;
    }
}
