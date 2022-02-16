using System.Collections.Concurrent;
using System.Reactive.Linq;
using Computer.Bus.Domain.Contracts;
using Computer.Client.Domain.Contracts.Model.ToDoList;
using DomainModels = Computer.Client.Domain.Contracts.Model;
using IExternalBus = Computer.Bus.Domain.Contracts.IBus;
using IExternalRequestService = Computer.Bus.Domain.Contracts.IRequestService;
using ExternalEvents = Computer.Client.Domain.Contracts.Bus.Events;
using InternalBus = Computer.Domain.Bus.Reactive.Contracts.IReactiveBus;
using InternalEvents = Computer.Client.Domain.Contracts.Bus.Events;
using InternalBusEvent = Computer.Domain.Bus.Reactive.Contracts.Model.IBusEvent;
using IInternalRequestService = Computer.Domain.Bus.Contracts.IRequestService;
using DtoModels = Computer.Client.App.RemoteApps.Model;

namespace Computer.Client.App.Bus;

public class ExternalRouter
{
    private readonly InternalBus _internalBus;
    private readonly IInternalRequestService _internalRequestService;
    private readonly IExternalBus _externalBus;
    private readonly IRequestService _externalRequestService;

    //private readonly ConcurrentDictionary<string, ExternalToInternalConfig> _externalToInternal;
    private readonly ConcurrentDictionary<string, InternalToExternalConfig> _internalToExternal = new();
    private readonly List<IDisposable> _subscriptions = new();

    public ExternalRouter(
        InternalBus internalBus,
        IInternalRequestService internalRequestService,
        IExternalBus externalBus,
        IExternalRequestService externalRequestService)
    {
        _internalBus = internalBus;
        _internalRequestService = internalRequestService;
        _externalBus = externalBus;
        _externalRequestService = externalRequestService;

        // _externalToInternal = new(new Dictionary<string, ExternalToInternalConfig>
        // {
        //     {Events.GetConnectionResponse, new ExternalToInternalConfig()},
        //     {Events.CloseConnectionResponse, new ExternalToInternalConfig()},
        // });

        _internalToExternal = new ConcurrentDictionary<string, InternalToExternalConfig>(
            new Dictionary<string, InternalToExternalConfig>
            {
                { InternalEvents.GetConnection, new InternalToExternalConfig(OnGetConnection, typeof(DomainModels.AppConnectionRequest)) },
                //{InternalEvents.CloseConnection, new InternalToExternalConfig(OnDisconnectRequest)},
                { InternalEvents.DefaultListRequest, new InternalToExternalConfig(OnDefaultListRequest, typeof(DomainModels.ToDoList.DefaultListRequest)) }
            });
    }

    private async Task<IPublishResult> OnDefaultListRequest(string subject, InternalBusEvent busEvent)
    {
        if (busEvent.Param == null)
        {
            return Computer.Bus.Domain.Contracts.PublishResult.CreateError(
                "failed trying to publish/route. param was null");
        }

        if (!typeof(DomainModels.ToDoList.DefaultListRequest).IsAssignableFrom(busEvent.Type))
        {
            return Computer.Bus.Domain.Contracts.PublishResult.CreateError(
                "failed trying to publish/route. types do not match");
        }

        var param = (DomainModels.ToDoList.DefaultListRequest)busEvent.Param;
        return await _externalBus.Publish<DomainModels.ToDoList.DefaultListRequest>(ExternalEvents.DefaultListRequest,
            param,
            null,
            busEvent.CorrelationId).ConfigureAwait(false);
    }

    public async Task RestartListening()
    {
        await StopListening().ConfigureAwait(false);
        var extenalSubs = new List<Task<Computer.Bus.Domain.Contracts.ISubscription>>();
        extenalSubs.AddRange(new[]
        {
            _externalBus.Subscribe<DomainModels.AppConnectionResponse>(ExternalEvents.GetConnectionResponse,
                OnConnectionResponse),
            //_externalBus.Subscribe<ExternalModels.AppDisconnectResponse>(ExternalEvents.CloseConnectionResponse, OnCloseResponse),
            _externalBus.Subscribe<DomainModels.ToDoList.DefaultListResponse>(
                ExternalEvents.DefaultListResponse,
                OnDefaultListResponse)
        });
        var subscriptions = await Task.WhenAll(extenalSubs).ConfigureAwait(false);
        _subscriptions.AddRange(subscriptions);
        
        //todo: wire up cancellation
        var cts = new CancellationTokenSource();
        
        var internalSubs = _internalToExternal.Select(internalToExtenalKvp =>
        {
            return _internalBus.Subscribe(internalToExtenalKvp.Key, internalToExtenalKvp.Value.InternalSubscribeType)
                .SelectMany(busEvent => Observable.FromAsync(async _ => await OnInternalEvent(busEvent, internalToExtenalKvp).ConfigureAwait(false)))
                .Subscribe();
        }).Append(Computer.Domain.Bus.Contracts.RequestServiceExtensions.Listen<DefaultListRequest, DefaultListResponse>(_internalRequestService, 
            InternalEvents.DefaultListRequest, InternalEvents.DefaultListResponse, 
            (q,r,s)=>OnInternalDefaultListRequest(q,r,s, cts.Token)));
        _subscriptions.AddRange(internalSubs);
    }

    private async Task<DefaultListResponse?> OnInternalDefaultListRequest(DefaultListRequest? param, 
        string eventId, string correlationId,
        CancellationToken cancellationToken)
    {
        if (param == null)
        {
            return new DomainModels.ToDoList.DefaultListResponse { Success = false };
        }
        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var timeoutToken = new CancellationTokenSource(); //todo: TimeSpan.FromSeconds(1));
        timeoutToken.Token.Register(() =>
        {
            cts.Cancel();
        });
        var externalResponse =
            await _externalRequestService.Request<DomainModels.ToDoList.DefaultListRequest, DomainModels.ToDoList.DefaultListResponse>(
                param,
                ExternalEvents.DefaultListRequest, ExternalEvents.DefaultListResponse,
                eventId: null,
                correlationId: correlationId, 
                cancellationToken: cts.Token).ConfigureAwait(false);
        if (!externalResponse.Success || externalResponse.Obj == null ||
            externalResponse.Obj.List == null)
        {
            return new DomainModels.ToDoList.DefaultListResponse { Success = false };
        }
        
        return externalResponse.Obj;
    }

    private async Task OnInternalEvent(InternalBusEvent busEvent,
        KeyValuePair<string, InternalToExternalConfig> internalToExtenalKvp)
    {
        var result = await internalToExtenalKvp.Value.InternalToExternalCallback(internalToExtenalKvp.Key, busEvent).ConfigureAwait(false);
    }

    private async Task<Computer.Bus.Domain.Contracts.IPublishResult> OnGetConnection(string subject, InternalBusEvent busEvent)
    {
        if (busEvent.Param == null)
        {
            return Computer.Bus.Domain.Contracts.PublishResult.CreateError(
                "failed trying to publish/route. param was null");
        }

        if (!typeof(DomainModels.AppConnectionRequest).IsAssignableFrom(busEvent.Type))
        {
            return Computer.Bus.Domain.Contracts.PublishResult.CreateError(
                "failed trying to publish/route. types do not match");
        }

        var param = (DomainModels.AppConnectionRequest)busEvent.Param;
        if (param.InstanceId == null)
        {
            return Computer.Bus.Domain.Contracts.PublishResult.CreateError(
                "failed trying to publish/route. instance id was null");
        }
        return await _externalBus.Publish<DomainModels.AppConnectionRequest>(ExternalEvents.GetConnection,
            param,
            null,
            busEvent.CorrelationId).ConfigureAwait(false);
    }

    /*private async Task<IPublishResult> OnDisconnectRequest(string subject, BusEvent busEvent)
    {
        if (busEvent.Param == null)
        {
            return PublishResult.CreateError("failed trying to publish/route. param was null");
        }
        if (busEvent.Type != typeof(InternalModels.AppDisconnectRequest))
        {
            return PublishResult.CreateError("failed trying to publish/route. types do not match");
        }

        var param = (InternalModels.AppDisconnectRequest)busEvent.Param;
        return await _externalBus.Publish(ExternalEvents.CloseConnection,
            param,
            eventId: null,
            correlationId: busEvent.CorrelationId);
    }*/

    public Task StopListening()
    {
        var subscriptions = _subscriptions.ToArray();
        _subscriptions.Clear();
        foreach (var subscription in subscriptions)
        {
            if (subscription == null) continue;

            try
            {
                subscription.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        return Task.CompletedTask;
    }

    private async Task OnConnectionResponse(DomainModels.AppConnectionResponse? param, string eventId,
        string correlationId)
    {
        if (param == null)
        {
            return;
        }

        await _internalBus.Publish(
            InternalEvents.GetConnectionResponse,
            typeof(DomainModels.AppConnectionResponse),
            param,
            null,
            correlationId).ConfigureAwait(false);
    }
    
    private async Task OnDefaultListResponse(DefaultListResponse? param, string eventId, string correlationId)
    {
        if (param == null)
        {
            return;
        }
        await _internalBus.Publish(
            InternalEvents.DefaultListResponse,
            typeof(DomainModels.ToDoList.DefaultListResponse),
            param,
            null,
            correlationId).ConfigureAwait(false);
    }

    // private async Task OnCloseResponse(InternalModels.AppDisconnectResponse? param, string eventid, string correlationid)
    // {
    //     if(param == null) return;
    //     await _internalBus.Publish(
    //         InternalEvents.CloseConnectionResponse,
    //         param,
    //         eventId: null,
    //         correlationid);
    // }

    //private record ExternalToInternalConfig();

    private record InternalToExternalConfig(
        Func<string, InternalBusEvent, Task<Computer.Bus.Domain.Contracts.IPublishResult>> InternalToExternalCallback,
        Type InternalSubscribeType);
    private record InternalToExternalRequestConfig<TInternalRequest, TInternalResponse, TExtenalRequest, TExternalResponse>(
        Func<string, InternalBusEvent, TInternalRequest> ToInternalRequest,
        Func<string, InternalBusEvent, TInternalRequest, TExtenalRequest> ToExtenalRequest,
        Type InternalSubscribeType);
}