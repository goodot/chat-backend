using AutoMapper;
using ChatAPI.Data;
using ChatAPI.Data.Models;
using ChatAPI.Domain;
using ChatAPI.Domain.Repository.Interfaces;
using ChatAPI.Models;
using ChatAPI.Models.Socket;
using ChatAPI.WebSockets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatAPI.Middleware
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketConnectionManager _manager;
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public WebSocketMiddleware(RequestDelegate next, WebSocketConnectionManager manager)
        {
            _next = next;
            _manager = manager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>() as UnitOfWork;
            _mapper = context.RequestServices.GetRequiredService<IMapper>();
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                var id = _manager.AddSocket(socket);

                if (id != null)
                {
                    if (socket.State == System.Net.WebSockets.WebSocketState.Open)
                        await socket.SendAsync(Encoding.UTF8.GetBytes(id), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

                }

                await Receive(socket, async (result, buffer) =>
                {
                    switch (result.MessageType)
                    {
                        case WebSocketMessageType.Text:
                            await RouteJSONMessageAsync(Encoding.UTF8.GetString(buffer, 0, result.Count));
                            return;
                        case WebSocketMessageType.Close:
                            var id = _manager.GetAll().FirstOrDefault(x => x.Value == socket).Key;
                            WebSocket sock;
                            _manager.GetAll().TryRemove(id, out sock);
                            await sock.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

                            return;
                    }
                });
            }
            else
            {
                await _next(context);
            }
        }
        private async Task RouteJSONMessageAsync(string message)
        {

            var routeOb = JsonConvert.DeserializeObject<SocketRequest>(message);
            Guid guidOutput;

            if (Guid.TryParse(routeOb.RoomId.ToString(), out guidOutput))
            {
                var sockets = _manager.GetAll().Where(s => s.Key == routeOb.RoomId.ToString());
                foreach(var sock in sockets)
                {
                    if (sock.Value.State == WebSocketState.Open)
                    {
                        await sock.Value.SendAsync(Encoding.UTF8.GetBytes(routeOb.Body.ToString()), WebSocketMessageType.Text, true, CancellationToken.None);
                        var msg = _mapper.Map<Message>(routeOb);
                        await _unitOfWork.MessageRepository.SendAsync(msg);
                        await _unitOfWork.CommitAsync();
                    }

                }
            }


        }
        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(
                    buffer: new ArraySegment<byte>(buffer),
                    cancellationToken: CancellationToken.None);
                handleMessage(result, buffer);
            }
        }

    }
}
