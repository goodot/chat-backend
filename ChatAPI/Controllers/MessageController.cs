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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly int _userId;

        public MessageController(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor,
            int? userId = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userId = userId ?? contextAccessor.HttpContext.User.Identity.ExtractUserId();
        }


        //GET api/v1/message/{id}
        [HttpGet("{id}", Name = nameof(GetMessageById))]
        public async Task<ActionResult<MessageDto>> GetMessageById(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
            if (message == null)
                return NotFound();
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
            return CreatedAtRoute(nameof(GetMessageById), new {Id = message.Id}, messageDto);
        }

        //GET api/v1/message/
        [HttpGet("{roomId}/{page}/{pageSize}")]
        public async Task<ActionResult<List<MessageDto>>> GetMessages(int roomId, int page, int pageSize)
        {
            return await Task.Factory.StartNew<ActionResult<List<MessageDto>>>(() =>
            {
                var messages = _unitOfWork.MessageRepository.GetPaged(roomId, page, pageSize);
                var messageDtos = _mapper.Map<IEnumerable<MessageDto>>(messages);
                return Ok(messageDtos);
            });
            
        }

        //GET api/v1/message/all/
        [HttpGet("all/{roomId}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesByRoom(int roomId)
        {
            return await Task.Factory.StartNew<ActionResult<IEnumerable<MessageDto>>>(() =>
            {
                var messages = _unitOfWork.MessageRepository.GetByRoom(roomId);
                var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messages);
                return Ok(messagesDto);
            });
        }
    }
}