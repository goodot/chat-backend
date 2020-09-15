using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatAPI.WebSockets
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public string AddSocket(WebSocket socket)
        {
            var id = Guid.NewGuid().ToString();
            _sockets.TryAdd(id, socket);
            return id;
        }
        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public WebSocket GetSocket(string id)
        {
            if (!_sockets.ContainsKey(id)) return null;
            return _sockets[id];
        }

       
    }
}
