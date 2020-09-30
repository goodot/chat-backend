using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Dto.Request
{
    public class SendMessageRequest
    {
        public int SenderId { get; set; }
        public int RoomId { get; set; }
        public string Body { get; set; }
    }
}
