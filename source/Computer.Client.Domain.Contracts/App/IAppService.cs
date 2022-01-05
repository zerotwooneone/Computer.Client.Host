namespace Computer.Client.Domain.Contracts.App;

public interface IAppService
{
    Task<string> JsonFunction(string appName, string methodName, string? param = null);
}