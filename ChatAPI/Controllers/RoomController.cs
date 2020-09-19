using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAPI.Domain.Repository;
using ChatAPI.Domain.Repository.Interfaces;
using ChatAPI.Models.Dto.Request;
using ChatAPI.Models.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        public RoomController(IUnitOfWork unitOfWork)
        {

        }
        [HttpPost]
        public ActionResult<RoomDto> CreateRoom(CreateRoomRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
