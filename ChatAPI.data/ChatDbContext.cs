using ChatAPI.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAPI.Data.Models
{
	public class ChatDbContext: DbContext
	{
		public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options)
		{
		}
		public DbSet<ChatMessage> Messages { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Room> Rooms { get; set; } 
	}
}
