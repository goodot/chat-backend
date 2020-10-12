using ChatAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository.Interfaces
{
    public interface ISocketRepository
    {
        Task AddSocketAsync(Socket socket);
    }
}
