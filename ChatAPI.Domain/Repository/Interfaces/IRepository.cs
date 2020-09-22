using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository.Interfaces
{
    public interface IRepository<T> where T: class
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        void Add(T entity);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }

}
