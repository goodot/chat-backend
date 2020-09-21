using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChatAPI.Data.Models;
using ChatAPI.Domain;
using ChatAPI.Domain.Repository;
using ChatAPI.Domain.Repository.Interfaces;
using ChatAPI.Extensions;
using ChatAPI.Models.Dto.Request;
using ChatAPI.Models.Dto.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ChatAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IConfiguration _config;
        public RoomController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork as UnitOfWork;
            _mapper = mapper;
            _config = config;
        }
        //POST api/v1/room
        [HttpPost]
        public async Task<ActionResult<RoomDto>> CreateRoom(CreateRoomRequest request)
        {
            var user = new User
            {
                Username = request.Username
            };
            _unitOfWork.UserRepository.Add(user);

            var token = user.GenerateToken(
                key: _config["Jwt:Key"],
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"]
                );
            var room = new Room
            {
                Description = request.RoomDescription,
                CreatorId = user.Id
            };
            await _unitOfWork.RoomRepository.CreateAsync(room);
            user.RoomId = room.Id;
            await _unitOfWork.CommitAsync();

            var roomDto = _mapper.Map<RoomDto>(room);
            var response = new CreateRoomResponse
            {
                Room = roomDto,
                Token = token
            };
            return CreatedAtRoute(nameof(GetRoomById), new { Id = room.Id }, response);
        }
        //GET api/v1/room
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<RoomDto>> GetRoomById(int id)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
            return Ok(room);

        }
        //GET api/v1/room/identity
        [HttpGet("identity/{identity}")]
        [Authorize]
        public async Task<ActionResult<RoomDto>> GetRoomByIdentity(string identity)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdentityAsync(identity);
            return Ok(room);

        }
    }
}
