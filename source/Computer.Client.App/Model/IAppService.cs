namespace Computer.Client.App.Model;

public interface IAppService
{
    Task<string> JsonFunction(string appName, string methodName, string? param = null);
}