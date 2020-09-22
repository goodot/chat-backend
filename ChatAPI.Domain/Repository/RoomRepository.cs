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
    public class RoomRepository : IRoomRepository
    {
        private readonly ChatDbContext _dbContext;

        public RoomRepository(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Close(int id)
        {
            var room = _dbContext.Rooms.Find(id);
            if (room == null)
                return;
            room.IsActive = false;
        }

        public async Task CloseAsync(int id)
        {
            var room = await _dbContext.Rooms.FindAsync(id);
            if (room == null)
                return;
            room.IsActive = false;
        }

        public void Create(Room room)
        {
            room.Identity = Guid.NewGuid().ToString();
            _dbContext.Add(room);
        }

        public async Task CreateAsync(Room room)
        {
            room.Identity = Guid.NewGuid().ToString();
            await _dbContext.AddAsync(room);
        }

        public void Delete(int id)
        {
            var room = _dbContext.Rooms.Find(id);
            if (room == null)
                return;
            _dbContext.Rooms.Remove(room);
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _dbContext.Rooms.FindAsync(id);
            if (room == null)
                return;
            _dbContext.Rooms.Remove(room);
        }

        public Room GetById(int id)
        {
            return _dbContext.Rooms.Find(id);
        }

        public async Task<Room> GetByIdAsync(int id)
        {
            var room = await _dbContext.Rooms.FindAsync(id);
            return room;
        }

        public Room GetByidentity(string identity)
        {
            return _dbContext.Rooms.FirstOrDefault(x => x.Identity == identity);

        }

        public async Task<Room> GetByIdentityAsync(string identity)
        {
            var room = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Identity == identity);
            return room;
        }

        public bool HasUser(int userId, int roomId)
        {
            var userExists = _dbContext.Users.Any(
                x => x.Id == userId
                &&
                x.RoomId == roomId);
            return userExists;
        }

        public async Task<bool> HasUserAsync(int userId, int roomId)
        {
            var userExists = await _dbContext.Users.AnyAsync(
                x => x.Id == userId
                &&
                x.RoomId == roomId);
            return userExists;
        }
    }
}
