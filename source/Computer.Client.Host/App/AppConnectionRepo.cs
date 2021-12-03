using System.Collections.Concurrent;

namespace Computer.Client.Host.App;

public class AppConnectionRepo : IAppConnectionRepo
{
    private readonly ConcurrentDictionary<string, AppConnectionDetails> _details = new();

    public async Task<AppConnectionDetails> GetOrCreate(string clientId, string instanceId)
    {
        if(_details.TryGetValue(clientId, out AppConnectionDetails? existing))
        {
            return existing;
        }
        //todo: call app, ask if instance id is ok
        var dx = new TaskCompletionSource<AppConnectionDetails>();
        _details.AddOrUpdate(clientId, 
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
        return await dx.Task;
    }

    public async Task<bool> TryDelete(string clientId, string instanceId)
    {
        return (await DeleteAllByClientId(clientId)).Any();
    }
    public async Task<IEnumerable<AppConnectionDetails>> DeleteAllByClientId(string clientId)
    {
        if (!_details.TryRemove(clientId, out var details))
        {
            return Enumerable.Empty<AppConnectionDetails>();
        }
        return new[] { details };
    }
}
