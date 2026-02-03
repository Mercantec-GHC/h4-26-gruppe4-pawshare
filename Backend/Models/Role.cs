using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Role : Common
    {
        public required string Title { get; set; }
        public List<UserRole>? Users { get; set; }
    }
}
