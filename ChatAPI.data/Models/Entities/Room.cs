using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ChatAPI.Data.Models.Entities
{
	public class Room
	{
		public int Id { get; set; }
		public string Description { get; set; }
		public int? CreatorId { get; set; }
		[DefaultValue(typeof(bool), "true")]
		public bool IsActive { get; set; }
		public string PassCode { get; set; }
		[DefaultValue(typeof(DateTime), "getdate()")]
		public DateTime CreatedAt { get; set; }
	}
}
