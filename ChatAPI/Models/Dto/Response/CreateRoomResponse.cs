using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Dto.Response
{
    public class CreateRoomResponse
    {
        public RoomDto Room { get; set; }
        public string Token { get; set; }
    }
}
