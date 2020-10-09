using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models
{
    public class SocketRequest
    {
        public int SenderId { get; set; }
        public string RoomId { get; set; }
        public string Body { get; set; }
        public string Token { get; set; }
    }
}
