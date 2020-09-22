using ChatAPI.Data;
using ChatAPI.Data.Models;
using ChatAPI.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ChatDbContext dbContext):base(dbContext)
        {

        }
        public IEnumerable<User> GetByRoom(int roomId)
        {
            var users = _dbContext.Users.Where(x => x.RoomId == roomId);
            return users;
        }

    }
}
