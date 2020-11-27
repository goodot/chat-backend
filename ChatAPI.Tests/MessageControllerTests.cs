using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ChatAPI.Controllers;
using ChatAPI.Data.Models;
using ChatAPI.Domain.Repository.Interfaces;
using ChatAPI.Models.Dto.Request;
using ChatAPI.Models.Dto.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ChatAPI.Tests
{
    [TestFixture]
    public class MessageControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IMapper> _mapper;
        private Mock<IHttpContextAccessor> _contextAccessor;
        private MessageController _controller;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _contextAccessor = new Mock<IHttpContextAccessor>();

            _controller = new MessageController(_unitOfWork.Object, _mapper.Object, _contextAccessor.Object, 1);
        }

        [Test]
        [TestCase(1, typeof(NotFoundResult))]
        [TestCase(2, typeof(OkObjectResult))]
        public async Task GetMessageById_WhenCalled_ReturnMessage(int id, Type expectedType)
        {
            _unitOfWork.Setup(u => u.MessageRepository.GetByIdAsync(1))
                .ReturnsAsync((Message) null);
            _unitOfWork.Setup(u => u.MessageRepository.GetByIdAsync(2))
                .ReturnsAsync(new Message {Id = 2});
            _mapper.Setup(m => m.Map<MessageDto>(It.IsAny<Message>()))
                .Returns(new MessageDto {Id = id});


            var result = await _controller.GetMessageById(id);

            Assert.That(result.Result, Is.TypeOf(expectedType));
        }

        [Test]
        public async Task SendMessage_WhenCalled_ReturnCreatedAtRoute()
        {
            _mapper.Setup(m => m.Map<Message>(It.IsAny<SendMessageRequest>())).Returns(new Message {Id = 1});
            _unitOfWork.Setup(u => u.MessageRepository.SendAsync(It.IsAny<Message>()));
            _unitOfWork.Setup(u => u.CommitAsync());
            _mapper.Setup(m => m.Map<MessageDto>(It.IsAny<Message>()))
                .Returns(new MessageDto {Id = 1});


            var result = await _controller.SendMessage(new SendMessageRequest
            {
                Body = "test",
                RoomId = 1
            });

            _unitOfWork.Verify(u => u.MessageRepository.SendAsync(It.IsAny<Message>()));
            _unitOfWork.Verify(u => u.CommitAsync());

            Assert.That(result.Result, Is.TypeOf<CreatedAtRouteResult>());
        }

        [Test]
        public void GetMessages_WhenCalled_ReturnsMessageList()
        {
            _unitOfWork.Setup(u => u.MessageRepository.GetPaged(1, 1, 1))
                .Returns(new List<Message>
                {
                    new Message
                    {
                        Id = 1
                    }
                });
            _mapper.Setup(m => m.Map<IEnumerable<MessageDto>>(It.IsAny<List<Message>>()))
                .Returns(new List<MessageDto>
                {
                    new MessageDto
                    {
                        Id = 1
                    }
                });

            var result = _controller.GetMessages(1, 1, 1);

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okObjectResult = (OkObjectResult) result.Result;
            var messages = (List<MessageDto>) okObjectResult.Value;
            Assert.That(messages.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetMessagesByRoom_WhenCalled_ReturnRooms()
        {
            _unitOfWork.Setup(u => u.MessageRepository.GetByRoom(1))
                .Returns(new List<Message> {new Message {Id = 1}});
            _mapper.Setup(m => m.Map<IEnumerable<MessageDto>>(It.IsAny<IEnumerable<Message>>()))
                .Returns(
                    new List<MessageDto>
                    {
                        new MessageDto
                        {
                            Id = 1
                        }
                    }
                );

            var result = _controller.GetMessagesByRoom(1);

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okObjectResult = (OkObjectResult) result.Result;
            var messages = (IEnumerable<MessageDto>) okObjectResult.Value;

            Assert.That(messages.Count, Is.EqualTo(1));
        }
    }
}