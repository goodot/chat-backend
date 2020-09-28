using ChatAPI.Data;
using ChatAPI.Data.Models;
using ChatAPI.Domain.Extensions;
using ChatAPI.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Domain.Repository
{
	public class MessageRepository : IMessageRepository
	{
		private readonly ChatDbContext _dbContext;

		public MessageRepository(ChatDbContext dbContext)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		}

		public IEnumerable<Message> Get(int page, int pageSize)
		{
			var messages = _dbContext.Messages.GetPaged(page, pageSize);
			return messages.Results;
		}
	}
}
