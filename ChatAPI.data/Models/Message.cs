using System;
using System.Collections.Generic;

namespace ChatAPI.Data.Models
{
    public partial class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RoomId { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Room Room { get; set; }
        public virtual User Sender { get; set; }
    }
}
