using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatData.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public int RoomId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
