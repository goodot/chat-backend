using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Data.Models.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        [Required]
        public int SenderId { get; set; }
        [Required]
        public int RoomId { get; set; }
        [Required]
        public string Text { get; set; }
        [DefaultValue(typeof(DateTime), "getdate()")]
        public DateTime? CreatedAt { get; set; }
    }
}
