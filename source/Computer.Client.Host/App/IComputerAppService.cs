namespace Computer.Client.Host.App;

public interface IComputerAppService
{
    Task ReStartListening();
    void StopListening();
}
