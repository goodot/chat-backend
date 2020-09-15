using ChatAPI.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Data.Models
{
	public class ChatDbContext: DbContext
	{
		public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options)
		{
		}
		DbSet<ChatMessage> Messages { get; set; }
		DbSet<User> Users { get; set; }
		DbSet<Room> Rooms { get; set; } 
	}
}
