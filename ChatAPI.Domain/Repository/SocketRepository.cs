using ChatAPI.Data;
using ChatAPI.Data.Models;
using ChatAPI.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddSocketAsync(Socket socket)
        {
            await _context.AddAsync(socket);
        }

        public async Task<IEnumerable<Socket>> GetByRoomIdentityAsync(string identity)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(a => a.Identity == identity);
            if (room == null)
                return null;
            var users = _context.Users.Where(a => a.RoomId == room.Id);
            var sockets = _context.Sockets.Where(a => users.Any(x => x.Id == a.UserId));
            return sockets;
        }

        public async Task<IEnumerable<Socket>> GetByRoomIdAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
                return null;
            var users = _context.Users.Where(a => a.RoomId == room.Id);
            var sockets = _context.Sockets.Where(a => users.Any(x => x.Id == a.UserId));
            return sockets;
        }
    }
}
