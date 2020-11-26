using ChatAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository.Interfaces
{
    public  interface IUserRepository:IRepository<User>
    {
        IEnumerable<User> GetByRoom(int roomId);
    }
}
