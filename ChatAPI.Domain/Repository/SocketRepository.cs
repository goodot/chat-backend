using ChatAPI.Data;
using ChatAPI.Data.Models;
using ChatAPI.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository
{
    public class SocketRepository : ISocketRepository
    {
        private ChatDbContext _context;

        public SocketRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task AddSocket(Socket socket)
        {
            await _context.AddAsync(socket);
        }
    }
}
