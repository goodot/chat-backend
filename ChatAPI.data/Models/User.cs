using System;
using System.Collections.Generic;

namespace ChatAPI.Data.Models
{
    public partial class User
    {
        public User()
        {
            Message = new HashSet<Message>();
            Room = new HashSet<Room>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public bool? IsActive { get; set; }
        public int? RoomId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<Room> Room { get; set; }
    }
}
