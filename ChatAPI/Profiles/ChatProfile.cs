using AutoMapper;
using ChatAPI.Data.Models;
using ChatAPI.Models.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Profiles
{
    public class ChatProfile: Profile
    {
        public ChatProfile()
        {
            CreateMap<Room, RoomDto>();
            CreateMap<User, UserDto>();
        }
    }
}
