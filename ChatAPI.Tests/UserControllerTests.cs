using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ChatAPI.Controllers;
using ChatAPI.Data.Models;
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
    public class UserControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IConfiguration> _configuration;
        private Mock<IMapper> _mapper;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _mapper = new Mock<IMapper>();
            _configuration = new Mock<IConfiguration>();
            _controller = new UserController(_unitOfWork.Object, _mapper.Object, _configuration.Object);
        }

        [Test]
        public async Task CreateUser_WrongRoom_ReturnsNotFound()
        {
            const string identity = "identity";
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdentityAsync(identity)).ReturnsAsync((Room) null);

            var result = await _controller.CreateUser(new CreateUserRequest {RoomIdentity = identity});

            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task CreateUser_CorrectRoom_CreatesUser()
        {
            const string identity = "identity";
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdentityAsync(identity))
                .ReturnsAsync(new Room {Id = 1, Identity = identity});
            _unitOfWork.Setup(u => u.UserRepository.AddAsync(It.IsAny<User>()));
            _unitOfWork.Setup(u => u.CommitAsync());
            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()));
            _configuration.Setup(c => c[It.IsAny<string>()]).Returns("test_test_test_test_test_test");

            var result = await _controller.CreateUser(new CreateUserRequest
            {
                RoomIdentity = identity,
                Username = "test"
            });

            _unitOfWork.Verify(u => u.UserRepository.AddAsync(It.IsAny<User>()));
            _unitOfWork.Verify(u => u.CommitAsync());

            Assert.That(result, Is.TypeOf<ActionResult<CreateUserResponse>>());
        }


        [Test]
        [TestCase(1, typeof(NotFoundResult))]
        [TestCase(2, typeof(OkObjectResult))]
        public async Task GetRoomById_WhenCalled_ReturnsRoomById(int id, Type expectedType)
        {
            _unitOfWork.Setup(u => u.UserRepository.GetByIdAsync(1))
                .ReturnsAsync((User) null);
            _unitOfWork.Setup(u => u.UserRepository.GetByIdAsync(2))
                .ReturnsAsync(new User {Id = 2});
            _mapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
                .Returns(new UserDto {Id = id});

            var result = await _controller.GetUserById(id);

            Assert.That(result.Result, Is.TypeOf(expectedType));
        }

        [Test]
        [TestCase(1, typeof(NotFoundResult))]
        [TestCase(2, typeof(OkObjectResult))]
        public async Task GetUsersByRoom_WhenCalled_ReturnsUsersByRoom(int roomId, Type expectedType)
        {
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdAsync(1)).ReturnsAsync((Room) null);
            _unitOfWork.Setup(u => u.RoomRepository.GetByIdAsync(2)).ReturnsAsync(new Room {Id = 2});
            _unitOfWork.Setup(u => u.UserRepository.GetByRoom(2))
                .Returns(new List<User>
                {
                    new User
                    {
                        Id = 1,
                        RoomId = 2
                    }
                });
            _mapper.Setup(m => m.Map<List<UserDto>>(It.IsAny<List<User>>()))
                .Returns(new List<UserDto>
                {
                    new UserDto
                    {
                        Id = 1,
                        RoomId = 2
                    }
                });
            var result = await _controller.GetUsersByRoom(roomId);
            Assert.That(result.Result, Is.TypeOf(expectedType));
        }
    }
}