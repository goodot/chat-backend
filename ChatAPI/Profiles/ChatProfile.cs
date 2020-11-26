using AutoMapper;
using ChatAPI.Data.Models;
using ChatAPI.Models.Socket;
using ChatAPI.Models.Dto.Request;
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
            CreateMap<Room, RoomDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Message, MessageDto>().ReverseMap();
            CreateMap<SendMessageRequest, Message>();
            CreateMap<SocketRequest, Message>().ReverseMap();
            CreateMap<SocketMessage, SocketRequest>().ReverseMap();

        }
    }
}
