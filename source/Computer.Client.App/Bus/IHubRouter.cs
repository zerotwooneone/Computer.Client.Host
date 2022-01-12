namespace Computer.Client.App.Bus;

public interface IHubRouter
{
    void ReStartListening();
    void StopListening();
}