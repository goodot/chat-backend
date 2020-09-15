using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Data
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RoomId { get; set; }
        public string Text { get; set; }
        public string Passcode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
