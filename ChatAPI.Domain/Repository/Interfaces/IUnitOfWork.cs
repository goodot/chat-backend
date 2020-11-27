using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoomRepository RoomRepository { get; }
        IMessageRepository MessageRepository { get; }
        void Commit();
        Task CommitAsync();
    }
}
