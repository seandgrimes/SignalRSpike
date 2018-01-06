using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRSpike.Hubs
{
    public class EchoHub : Hub
    {
      public Task Echo(string message) => Clients.All.InvokeAsync("Message", message);
    }
}