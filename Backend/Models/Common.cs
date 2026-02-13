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
        public string Id { get; set; }
        public  DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
