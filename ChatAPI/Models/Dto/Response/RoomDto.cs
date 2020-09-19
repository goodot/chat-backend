using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models.Dto.Response
{
	public class RoomDto
	{
        public int Id { get; set; }
        public string Description { get; set; }
        public int CreatorId { get; set; }
        public bool? IsActive { get; set; }
        public string PassCode { get; set; }
        public string Identity { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
