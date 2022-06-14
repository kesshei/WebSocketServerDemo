using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// webSocket
    /// </summary>
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="next"></param>
        /// <param name="actionPathName">目标路径</param>
        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// 中间件调用
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
                await new WebSocketHelper().WebSocketReceive(socket);
            }
            catch (Exception)
            {
            }
            await _next(httpContext);
        }
    }
}
