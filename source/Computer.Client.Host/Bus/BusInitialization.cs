using Computer.Client.Host.App;

namespace Computer.Client.Host.Bus;

public class BusInitialization : IHostedService
{
    private readonly IHubRouter _hubRouter;
    private readonly IComputerAppService computerAppService;
    private readonly ExternalRouter _externalRouter;

    public BusInitialization(IHubRouter hubRouter,
        IComputerAppService computerAppService,
        ExternalRouter externalRouter)
    {
        this._hubRouter = hubRouter;
        this.computerAppService = computerAppService;
        _externalRouter = externalRouter;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _hubRouter.ReStartListening();
        await computerAppService.ReStartListening();
        await _externalRouter.RestartListening();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _hubRouter.StopListening();
        computerAppService.StopListening();
        await _externalRouter.StopListening();
    }
}
