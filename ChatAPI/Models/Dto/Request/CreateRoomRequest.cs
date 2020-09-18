using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Dto.Request
{
    public class CreateRoomRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string RoomDescription { get; set; }

    }
}
