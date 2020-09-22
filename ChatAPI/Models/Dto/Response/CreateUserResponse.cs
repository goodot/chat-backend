using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Dto.Response
{
    public class CreateUserResponse
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
