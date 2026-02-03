using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class UserRole
    {
        [Required(ErrorMessage = "Der skal være mindst en tilkoblet bruger")]
        public required string UserId { get; set; }
        public User? User { get; set; }

        [Required(ErrorMessage = "Der skal være mindst en tilkoblet rolle")]
        public required string RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
