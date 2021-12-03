namespace Computer.Client.Host.App;

public interface IAppConnectionRepo
{
    Task<AppConnectionDetails> GetOrCreate(string clientId, string instanceId);
    Task<bool> TryDelete(string clientId, string instanceId);
    Task<IEnumerable<AppConnectionDetails>> DeleteAllByClientId(string clientId);
}
