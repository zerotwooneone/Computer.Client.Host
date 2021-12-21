using System.Collections.Concurrent;
using Computer.Bus.Contracts.Models;
using InternalBus = Computer.Client.Host.Bus.IBus;
using IExternalBus = Computer.Bus.Contracts.IBusClient;
using InternalEvents = Computer.Client.Host.App.Events;
using ExternalEvents = Computer.Apps.ToDoList.Contracts.Bus.Events;
using ExternalModels = Computer.Apps.ToDoList.Contracts.Model;
using InternalModels = Computer.Client.Host.App;

namespace Computer.Client.Host.Bus;

public class ExternalRouter
{
    private readonly IBus _internalBus;
    private readonly IExternalBus _externalBus;

    //private readonly ConcurrentDictionary<string, ExternalToInternalConfig> _externalToInternal;
    private readonly ConcurrentDictionary<string, InternalToExternalConfig> _internalToExternal = new();
    private readonly List<IDisposable> _subscriptions = new();

    public ExternalRouter(
        InternalBus internalBus,
        IExternalBus externalBus)
    {
        _internalBus = internalBus;
        _externalBus = externalBus;
        
        // _externalToInternal = new(new Dictionary<string, ExternalToInternalConfig>
        // {
        //     {Events.GetConnectionResponse, new ExternalToInternalConfig()},
        //     {Events.CloseConnectionResponse, new ExternalToInternalConfig()},
        // });
        
        _internalToExternal = new(new Dictionary<string, InternalToExternalConfig>
        {
            {InternalEvents.GetConnection, new InternalToExternalConfig(OnGetConnection)},
            {InternalEvents.CloseConnection, new InternalToExternalConfig(OnDisconnectRequest)},
        });
    }
    public async Task RestartListening()
    {
        await StopListening();
        var extenalSubs = new List<Task<ISubscription>>();
        extenalSubs.AddRange(new[]
        {
            _externalBus.Subscribe<ExternalModels.AppConnectionResponse>(ExternalEvents.GetConnectionResponse, OnConnectionResponse),
            _externalBus.Subscribe<ExternalModels.AppDisconnectResponse>(ExternalEvents.CloseConnectionResponse, OnCloseResponse),
        });
        var subscriptions = await Task.WhenAll(extenalSubs);
        _subscriptions.AddRange(subscriptions);

        var internalSubs = _internalToExternal.Select(internalToExtenalKvp =>
        {
            return _internalBus.Subscribe(internalToExtenalKvp.Key,
                busEvent => OnInternalEvent(busEvent, internalToExtenalKvp));
        });
        _subscriptions.AddRange(internalSubs);
    }

    private async Task OnInternalEvent(BusEvent busEvent, KeyValuePair<string,InternalToExternalConfig> internalToExtenalKvp)
    {
        var result = await internalToExtenalKvp.Value.internalToExternalCallback(internalToExtenalKvp.Key, busEvent);
    }

    private async Task<IPublishResult> OnGetConnection(string subject, BusEvent busEvent)
    {
        if (busEvent.Param == null)
        {
            return PublishResult.CreateError("failed trying to publish/route. param was null");
        }
        if (busEvent.Type != typeof(InternalModels.AppConnectionRequest))
        {
            return PublishResult.CreateError("failed trying to publish/route. types do not match");
        }

        var param = (InternalModels.AppConnectionRequest)busEvent.Param;
        return await _externalBus.Publish(ExternalEvents.GetConnection,
            new ExternalModels.AppConnectionRequest { AppId = param.appId, InstanceId = param.instanceId },
            eventId: null,
            correlationId: busEvent.CorrelationId);
    }

    private async Task<IPublishResult> OnDisconnectRequest(string subject, BusEvent busEvent)
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
            new ExternalModels.AppDisconnectRequest { AppId = param.appId, InstanceId = param.instanceId },
            eventId: null,
            correlationId: busEvent.CorrelationId);
    }

    public async Task StopListening()
    {
        var subscriptions = _subscriptions.ToArray();
        _subscriptions.Clear();
        foreach (var subscription in subscriptions)
        {
            if (subscription == null)
            {
                continue;
            }

            try
            {
                subscription.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    
    private async Task OnConnectionResponse(ExternalModels.AppConnectionResponse? param, string eventid, string correlationid)
    {
        if(param == null) return;
        await _internalBus.Publish(
            InternalEvents.GetConnectionResponse,
            new InternalModels.AppConnectionResponse(param.InstanceId),
            eventId: null,
            correlationid);
    }
    
    private async Task OnCloseResponse(ExternalModels.AppDisconnectResponse? param, string eventid, string correlationid)
    {
        if(param == null) return;
        await _internalBus.Publish(
            InternalEvents.CloseConnectionResponse,
            new InternalModels.AppConnectionResponse("disconnected without id"),
            eventId: null,
            correlationid);
    }

    //private record ExternalToInternalConfig();

    private record InternalToExternalConfig(Func<string, BusEvent, Task<IPublishResult>> internalToExternalCallback);
}

