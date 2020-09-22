using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository.Interfaces
{
    public interface IUnitOfWork:IDisposable
    {
        void Commit();
        Task CommitAsync();
    }
}
