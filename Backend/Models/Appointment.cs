using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Appointment : Common
    {
        public required DateTime Start {  get; set; }
        public required DateTime End { get; set; }
        public required string Address { get; set; }
        public required string Description { get; set; }
        public required List<string> AnimalIds { get; set; }
        public List<Animal>? Animals { get; set; }
        public required List<string> UserIds { get; set; }
        public List<Animal>? Users { get; set; }

    }
}
