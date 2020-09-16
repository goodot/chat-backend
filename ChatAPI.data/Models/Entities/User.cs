using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Data.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [DefaultValue(typeof(bool), "true")]
        public bool IsActive { get; set; }
        public int RoomId { get; set; }
        [DefaultValue(typeof(DateTime), "getdate()")]
        public DateTime CreatedAt { get; set; }
    }
}
