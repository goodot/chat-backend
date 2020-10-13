using AutoMapper;
using ChatAPI.Data;
using ChatAPI.Data.Models;
using ChatAPI.Domain;
using ChatAPI.Domain.Repository.Interfaces;
using ChatAPI.Extensions;
using ChatAPI.Models;
using ChatAPI.Models.Socket;
using ChatAPI.WebSockets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private const string Token = "token";

        private readonly RequestDelegate _next;
        private WebSocketConnectionManager _manager;
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IConfiguration _config;


        public WebSocketMiddleware(RequestDelegate next, WebSocketConnectionManager manager)
        {
            _next = next;
            _manager = manager;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            _unitOfWork = context.RequestServices.GetRequiredService<IUnitOfWork>() as UnitOfWork;
            _mapper = context.RequestServices.GetRequiredService<IMapper>();
            _config = context.RequestServices.GetRequiredService<IConfiguration>();


            var token = context.Request.Query.Extract(Token);



            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next(context);
                return;
            }

            if (token == null)
            {
                await _next(context);
                return;
            }

            if (!Extensions.Extensions.AuthenticateJwt(token, _config))
            {
                await _next(context);
                return;
            }


            var roomId = Convert.ToInt32(token.GetIdentityFromToken(_config).ExtractRoomId());
            var userId = Convert.ToInt32(token.GetIdentityFromToken(_config).ExtractUserId());


            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var id = _manager.AddSocket(socket);

            var dbSocket = new Socket
            {
                SocketId = id,
                UserId = userId
            };
            await _unitOfWork.SocketRepository.AddSocketAsync(dbSocket);
            await _unitOfWork.CommitAsync();

            if (id != null)
            {
                if (socket.State == WebSocketState.Open)
                    await socket.SendAsync(Encoding.UTF8.GetBytes(id), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            await Receive(socket, async (result, buffer) =>
            {
                switch (result.MessageType)
                {
                    case WebSocketMessageType.Text:
                        await RouteJSONMessageAsync(Encoding.UTF8.GetString(buffer, 0, result.Count), roomId, userId);
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
        private async Task RouteJSONMessageAsync(string message, int roomId, int userId)
        {
            try
            {
                #region get username
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
                if (user == null)
                    return;
                #endregion
                var socketMessage = new SocketMessage
                {
                    Body = message,
                    UserId = userId,
                    Username = user.Username
                };

                var msg = new Message
                {
                    RoomId = roomId,
                    SenderId = userId,
                    Body = message
                };
                msg.RoomId = roomId;
                msg.SenderId = userId;
                await _unitOfWork.MessageRepository.SendAsync(msg);
                await _unitOfWork.CommitAsync();


                var socketMessageString = JsonConvert.SerializeObject(socketMessage);

                var dbSockets = await _unitOfWork.SocketRepository.GetByRoomIdAsync(roomId);
                var sockets = _manager.GetAll().Where(s => dbSockets.Any(t => t.SocketId == s.Key));

                foreach (var sock in sockets)
                {
                    if (sock.Value.State == WebSocketState.Open)
                    {
                        await sock.Value.SendAsync(Encoding.UTF8.GetBytes(socketMessageString), WebSocketMessageType.Text, true, CancellationToken.None);
                    }


                }
            }
            catch (Exception ex)
            {
                //TODO log
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
