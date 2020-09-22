using ChatAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository.Interfaces
{
    public interface IRoomRepository
    {
        Task CreateAsync(Room room);
        void Create(Room room);
        Task<Room> GetByIdentityAsync(string identity);
        Room GetByidentity(string identity);
        Task<Room> GetByIdAsync(int id);
        Room GetById(int id);
        Task CloseAsync(int id);
        void Close(int id);
        Task DeleteAsync(int id);
        void Delete(int id);
        bool HasUser(int userId, int roomId);
        Task<bool> HasUserAsync(int userId, int roomId);
        

    }
}
