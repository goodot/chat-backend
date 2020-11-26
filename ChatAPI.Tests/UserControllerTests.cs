using AutoMapper;
using ChatAPI.Controllers;
using ChatAPI.Domain.Repository.Interfaces;
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
        
        
        
    }
}