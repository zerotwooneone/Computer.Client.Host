using Computer.Client.Host.App;

namespace Computer.Client.Host.Bus;

public class BusInitialization : IHostedService
{
    private readonly IHubRouter _hubRouter;
    private readonly IComputerAppService computerAppService;

    public BusInitialization(IHubRouter hubRouter,
        IComputerAppService computerAppService)
    {
        this._hubRouter = hubRouter;
        this.computerAppService = computerAppService;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _hubRouter.ReStartListening();
        await computerAppService.ReStartListening();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _hubRouter.StopListening();
        computerAppService.StopListening();
        return Task.CompletedTask;
    }
}
