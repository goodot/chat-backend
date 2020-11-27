using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChatAPI.Controllers;
using ChatAPI.Data.Models;
using ChatAPI.Domain;
using ChatAPI.Domain.Repository.Interfaces;
using ChatAPI.Models.Dto.Request;
using ChatAPI.Models.Dto.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace ChatAPI.Tests
{
    [TestFixture]
    public class RoomControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IConfiguration> _configuration;
        private Mock<IMapper> _mapper;
        private RoomController _controller;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _configuration = new Mock<IConfiguration>();
            _controller = new RoomController(_unitOfWork.Object, _mapper.Object, _configuration.Object);
        }

        [Test]
        public async Task CreateRoom_MakeCreateRoomRequest_ReturnsActionResultWithCreateRoomResponse()
        {
            _unitOfWork.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>()));
            _unitOfWork.Setup(u => u.UserRepository.Update(It.IsAny<User>()));
            _unitOfWork.Setup(u => u.CommitAsync());
            _unitOfWork.Setup(u => u.RoomRepository.CreateAsync(It.IsAny<Room>()));
            _mapper.Setup(m => m.Map<RoomDto>(It.IsAny<Room>())).Returns(It.IsAny<RoomDto>());
            _configuration.Setup(c => c[It.IsAny<string>()]).Returns("godot_chat_backend_project_sample_key");


            var result = await _controller.CreateRoom(new CreateRoomRequest
            {
                Username = "TestUser",
                RoomDescription = "TestRoom"
            });

            _unitOfWork.Verify(u => u.UserRepository.AddAsync(It.IsAny<User>()));
            _unitOfWork.Verify(u => u.UserRepository.Update(It.IsAny<User>()));
            _unitOfWork.Verify(u => u.CommitAsync());
            _unitOfWork.Verify(u => u.RoomRepository.CreateAsync(It.IsAny<Room>()));

            Assert.That(result, Is.TypeOf<ActionResult<CreateRoomResponse>>());
        }

        [Test]
        public async Task CloseRoom_CorrectId_ReturnsNoContent()
        {
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdAsync(1)).ReturnsAsync(new Room {Id = 1});
            _unitOfWork.Setup(u => u.RoomRepository.CloseAsync(It.IsAny<int>()));

            var result = await _controller.CloseRoom(1);
            _unitOfWork.Verify(u => u.RoomRepository.CloseAsync(It.IsAny<int>()));

            Assert.That(result, Is.TypeOf<NoContentResult>());
        }

        [Test]
        public async Task CloseRoom_WrongId_ReturnsNotFound()
        {
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdAsync(1)).ReturnsAsync((Room)null);

            var result = await _controller.CloseRoom(1);

            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task GetRoomById_WrongId_ReturnsNotFound()
        {
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdAsync(1)).ReturnsAsync((Room) null);

            var result = await _controller.GetRoomById(1);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task GetRoomById_CorrectId_ReturnsRoom()
        {
            var room = new Room {Id = 1};
            var roomDto = new RoomDto {Id = 1};
        
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdAsync(1)).ReturnsAsync(room);
            _mapper.Setup(u => u.Map<RoomDto>(room)).Returns(roomDto);

            var result = await _controller.GetRoomById(1);
            var response = (OkObjectResult) result.Result;
            Assert.That(response.Value, Is.EqualTo(roomDto));
        }
        [Test]
        public async Task GetRoomByIdentity_WrongId_ReturnsNotFound()
        {
            const string identity = "identity";
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdentityAsync(identity)).ReturnsAsync((Room) null);

            var result = await _controller.GetRoomByIdentity(identity);

            Assert.That(result.Result, Is.TypeOf<NotFoundResult>());
        }

        [Test]
        public async Task GetRoomByIdentity_CorrectId_ReturnsRoom()
        {
            var room = new Room {Id = 1};
            var roomDto = new RoomDto {Id = 1};
            const string identity = "identity";
            
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdentityAsync(identity)).ReturnsAsync(room);
            _mapper.Setup(u => u.Map<RoomDto>(room)).Returns(roomDto);

            var result = await _controller.GetRoomByIdentity(identity);
            var response = (OkObjectResult) result.Result;
            Assert.That(response.Value, Is.EqualTo(roomDto));
        }
        
        
    }
}