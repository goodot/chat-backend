using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChatAPI.Data.Models;
using ChatAPI.Domain;
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
    public class UserController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IConfiguration _config;
        public UserController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork as UnitOfWork;
            _mapper = mapper;
            _config = config;
        }

        //POST api/v1/user
        [HttpPost]
        public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest request)
        {
            Room room;
            if (request.RoomId != null)
            {
                room = await _unitOfWork.RoomRepository.GetByIdAsync(request.RoomId.Value);
            }
            else
            {
                room = await _unitOfWork.RoomRepository.GetByIdentityAsync(request.RoomIdentity);
            }
            if (room == null)
                return NotFound("Room not found");

            //check if username is unique in the room
            var userExists = _unitOfWork.UserRepository.GetByRoom(room.Id).Any(x=>x.Username == request.Username);
            if (userExists)
                return Conflict("user with this username already exists in the room");

            var user = new User
            {
                Username = request.Username
            };
            await _unitOfWork.UserRepository.AddAsync(user);
            var userDto = _mapper.Map<UserDto>(user);
            var response = new CreateUserResponse
            {
                Token = user.GenerateToken(
                    key: _config["Jwt:Key"],
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"]
                    ),
                User = userDto
            };
            return CreatedAtAction(nameof(GetUserById), new { Id = user.Id }, response);
        }
        //GET api/v1/user
        [Authorize]
        [HttpGet("{id}", Name = nameof(GetUserById))]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }
        //GET api/v1/user/by-room
        [Authorize]
        [HttpGet("{roomId}")]
        public ActionResult<List<UserDto>> GetUsersByRoom(int roomId)
        {
            var users = _unitOfWork.UserRepository.GetByRoom(roomId);
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }
    }
}
