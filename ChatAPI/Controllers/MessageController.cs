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
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly UnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IConfiguration _config;
        private int _userId;
        public MessageController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork as UnitOfWork;
            _mapper = mapper;
            _config = config;
            _userId = contextAccessor.HttpContext.User.Identity.ExtractUserId();
        }
        //GET api/v1/message/{id}
        [HttpGet("{id}", Name = nameof(GetMessageById))]
        public async Task<ActionResult<MessageDto>> GetMessageById(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
            var messageDto = _mapper.Map<MessageDto>(message);
            return Ok(messageDto);
        }
        //POST api/v1/message/send
        [HttpPost("send")]
        public async Task<ActionResult<MessageDto>> SendMessage(SendMessageRequest request)
        {
            var message = _mapper.Map<Message>(request);
            message.SenderId = _userId;
            await _unitOfWork.MessageRepository.SendAsync(message);
            await _unitOfWork.CommitAsync();
            var messageDto = _mapper.Map<MessageDto>(message);
            return CreatedAtRoute(nameof(GetMessageById), new { Id = message.Id }, messageDto);

        }
        //GET api/v1/message/
        [HttpGet("{roomId}/{page}/{pageSize}")]
        public ActionResult<List<MessageDto>> GetMessages(int roomId, int page, int pageSize)
        {
            var messages = _unitOfWork.MessageRepository.GetPaged(roomId, page, pageSize);
            var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);
            return Ok(messageDtos);
        }
        //GET api/v1/message/all/
        [HttpGet("all/{roomId}")]
        public ActionResult<IEnumerable<MessageDto>> GetMessagesByRoom(int roomId)
        {
            var messages = _unitOfWork.MessageRepository.GetByRoom(roomId);
            var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messages);
            return Ok(messagesDto);
        }
    }
}
