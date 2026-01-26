using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Common
    {
        [Key]
        public required string Id { get; set; }
        public required DateTime createdAt { get; set; }
        public required DateTime updatedAt { get; set; }
    }
}
