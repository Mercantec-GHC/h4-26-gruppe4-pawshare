using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Message : Common
    {
        public required string Content { get; set; }
        public required string UserId { get;set; }
        public required User User { get; set; }
        public required string ChatId { get; set; }
        public required Chat chat { get; set; }
    }
}
