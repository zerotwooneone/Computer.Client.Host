namespace Computer.Client.Domain;

public interface IAppService
{
    Task<string> JsonFunction(string appName, string methodName, string? param = null);
}
