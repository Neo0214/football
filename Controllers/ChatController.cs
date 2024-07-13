using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace football.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private WebSocketHandler _webSocketHandler;

        public ChatController(WebSocketHandler webSocketHandler)
        {
            _webSocketHandler = webSocketHandler;
        }

        [HttpGet("ws")]
        public async Task GetWebSocket(string userId)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                // Handle the WebSocket connection
                await _webSocketHandler.HandleWebSocketAsync(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }

    public class WebSocketHandler
    {
        public async Task HandleWebSocketAsync(WebSocket webSocket)
        {
            // Handle incoming and outgoing messages here
            var buffer = new byte[1024 * 4];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    // Process the received message, e.g., broadcast to other clients
                    Console.WriteLine($"Received message: {message}");

                    // Echo back the message
                    var echoBuffer = new ArraySegment<byte>(buffer, 0, result.Count);
                    await webSocket.SendAsync(echoBuffer, result.MessageType, result.EndOfMessage, CancellationToken.None);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
            }
        }
    }
}

