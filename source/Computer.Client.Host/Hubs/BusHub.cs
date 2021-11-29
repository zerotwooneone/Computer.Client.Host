using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Computer.Client.Host.Hubs
{
    public class BusHub: Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}