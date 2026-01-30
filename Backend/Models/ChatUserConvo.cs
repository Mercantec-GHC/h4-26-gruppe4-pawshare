using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class ChatUserConvo : Common
    {
        [Required(ErrorMessage = "Der skal være mindst en tilkoblet chat")]
        public required string ChatId { get; set; }
        public Chat? Chat { get; set; }

        [Required(ErrorMessage = "Der skal være mindst en tilkoblet bruger")]
        public required string UserId { get; set; }
        public User? User{ get; set; }
    }
}
