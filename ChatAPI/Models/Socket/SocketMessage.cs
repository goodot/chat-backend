using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Socket
{
    public class SocketMessage
    {
        public string Username { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
    }
}
