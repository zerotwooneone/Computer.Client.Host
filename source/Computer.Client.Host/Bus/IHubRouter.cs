namespace Computer.Client.Host.Bus;

public interface IHubRouter
{
    void ReStartListening();
    void StopListening();
}