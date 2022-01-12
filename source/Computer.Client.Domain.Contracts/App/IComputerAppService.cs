namespace Computer.Client.Domain.Contracts.App;

public interface IComputerAppService
{
    Task ReStartListening();
    void StopListening();
}