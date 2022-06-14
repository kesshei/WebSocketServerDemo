using System;
using System.Buffers;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class WebSocketHelper
    {
        public async Task WebSocketReceive(WebSocket webSocket)
        {
            var id = Guid.NewGuid().ToString("N");
            var buffer = ArrayPool<byte>.Shared.Rent(1024);
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        throw new WebSocketException(WebSocketError.ConnectionClosedPrematurely, result.CloseStatusDescription);
                    }
                    var text = Encoding.UTF8.GetString(buffer.AsSpan(0, result.Count));
                    var sendStr = Encoding.UTF8.GetBytes($"服务端 {id} : {text}  -{DateTime.Now}");
                    await webSocket.SendAsync(sendStr, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}
