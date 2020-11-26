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
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IConfiguration _config;
        public RoomController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }
        //POST api/v1/room
        [HttpPost]
        public async Task<ActionResult<CreateRoomResponse>> CreateRoom(CreateRoomRequest request)
        {
            var user = new User
            {
                Username = request.Username
            };
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();
            
            var room = new Room
            {
                Description = request.RoomDescription,
                CreatorId = user.Id
            };
            await _unitOfWork.RoomRepository.CreateAsync(room);
            await _unitOfWork.CommitAsync();
            user.RoomId = room.Id;
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CommitAsync();

            var token = user.GenerateToken(
                key: _config["Jwt:Key"],
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"]
                );

            var roomDto = _mapper.Map<RoomDto>(room);
            var response = new CreateRoomResponse
            {
                Room = roomDto,
                Token = token
            };

            return CreatedAtRoute(nameof(GetRoomById), new { Id = room.Id }, response);
        }
        //GET api/v1/room
        [HttpGet("{id}", Name = nameof(GetRoomById))]
        [Authorize]
        public async Task<ActionResult<RoomDto>> GetRoomById(int id)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
            if (room == null)
                return NotFound();
            return Ok(_mapper.Map<RoomDto>(room));

        }
        //GET api/v1/room/identity
        [HttpGet("identity/{identity}")]
        [Authorize]
        public async Task<ActionResult<RoomDto>> GetRoomByIdentity(string identity)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdentityAsync(identity);
            if (room == null)
                return NotFound();
            return Ok(room);

        }
        //DELETE api/v1/room
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> CloseRoom(int id)
        {
            var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
            if (room == null)
                return NotFound();
            await _unitOfWork.RoomRepository.CloseAsync(id);
            return NoContent();
        }
    }
}
