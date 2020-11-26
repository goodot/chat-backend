using System;
using System.Collections.Generic;

namespace ChatAPI.Data.Models
{
    public partial class Socket
    {
        public int Id { get; set; }
        public string SocketId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
