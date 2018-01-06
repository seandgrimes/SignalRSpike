using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRSpike.Hubs;

namespace SignalRSpike.Controllers
{
  [Route("api/[controller]")]
  public class ChatController : Controller
  {
    private readonly HubLifetimeManager<EchoHub> _echoLifetimeManager;

    public ChatController(HubLifetimeManager<EchoHub> echoLifetimeManager)
    {
      _echoLifetimeManager = echoLifetimeManager;
    }

    public async Task Post([FromBody]ChatMessage message)
    {
      var hub = new HubContext<EchoHub>(_echoLifetimeManager);
      await hub.Clients.All.InvokeAsync("Message", message.Message);
    }

    public class ChatMessage
    {
      public string Message { get; set; }
    }
  }
}
