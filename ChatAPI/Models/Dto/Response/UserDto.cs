using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Dto.Response
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool? IsActive { get; set; }
        public int? RoomId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
