namespace Computer.Client.App.Hubs;

public interface IBusHub
{
    Task EventToFrontEnd(EventForFrontEnd @event);
}