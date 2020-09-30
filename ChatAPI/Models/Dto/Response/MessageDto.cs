using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Dto.Response
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RoomId { get; set; }
        public string Body { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
