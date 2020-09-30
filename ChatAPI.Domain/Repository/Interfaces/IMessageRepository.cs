using ChatAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository.Interfaces
{
	public interface IMessageRepository
	{
		IEnumerable<Message> GetPaged(int page, int pageSize);
		Task SendAsync(Message message);
		Task<Message> GetByIdAsync(int id);
	}
}
