using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatAPI.Data.Models;
using ChatAPI.Data.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private ChatDbContext _context;
        public TestController(ChatDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<User>> Test()
        {
            return await _context.Users.FirstOrDefaultAsync();
        }
    }
}
