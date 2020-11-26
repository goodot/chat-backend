using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Dto.Request
{
    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string RoomIdentity { get; set; }
    }
}
