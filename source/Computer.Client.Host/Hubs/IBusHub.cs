namespace Computer.Client.Host.Hubs;

public interface IBusHub
{
    Task EventToFrontEnd(EventForFrontEnd @event);
}