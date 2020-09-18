using System;
using System.Collections.Generic;

namespace ChatAPI.Data.Models
{
    public partial class Room
    {
        public Room()
        {
            Message = new HashSet<Message>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public bool? IsActive { get; set; }
        public string PassCode { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User Creator { get; set; }
        public virtual ICollection<Message> Message { get; set; }
    }
}
