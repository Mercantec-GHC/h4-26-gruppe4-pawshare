using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs
{
    public class AnimalBookingDto
    {
        public required string AppointmentId { get; set; }
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
        public required string Address { get; set; }
    }
}
