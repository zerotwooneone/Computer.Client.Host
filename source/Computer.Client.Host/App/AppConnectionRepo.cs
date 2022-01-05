using System.Collections.Concurrent;

namespace Computer.Client.Host.App;

public class AppConnectionRepo : IAppConnectionRepo
{
    private readonly ConcurrentDictionary<AppConnection, AppConnectionDetails> _details = new();

    public Task<AppConnectionDetails> GetOrCreate(string appId, string clientId, string instanceId)
    {
        var appConnection = new AppConnection(appId, clientId, instanceId);
        if (_details.TryGetValue(appConnection, out var existing)) return Task.FromResult(existing);
        //todo: call app, ask if instance id is ok
        var dx = new TaskCompletionSource<AppConnectionDetails>();
        _details.AddOrUpdate(appConnection,
            s =>
            {
                var d = new AppConnectionDetails { InstanceId = instanceId };
                dx.SetResult(d);
                return d;
            },
            (s, d) =>
            {
                //this only allows for singleton app instances per client
                //discard submitted instance id, maybe publish an event or log
                dx.SetResult(d);
                return d;
            });
        return dx.Task;
    }

    public Task<bool> TryDelete(string appId, string clientId, string instanceId)
    {
        var appConnection = new AppConnection(appId, clientId, instanceId);
        return Task.FromResult(_details.TryRemove(appConnection, out _));
    }

    public Task<IEnumerable<AppConnectionDetails>> DeleteAllByClientId(string clientId)
    {
        var d = _details.Where(kvp => kvp.Key.clientId == clientId);
        foreach (var kvp in d) _details.TryRemove(kvp.Key, out _);
        return Task.FromResult(d.Select(kvp => kvp.Value));
    }

    internal record AppConnection(string appId, string clientId, string instanceId);
}