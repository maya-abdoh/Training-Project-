using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Fleet_Management_system;

namespace Fleet_Management_system
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketController : ControllerBase
    {
        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await WebSocketUtilities.Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }
}
