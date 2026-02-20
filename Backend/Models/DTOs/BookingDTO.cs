using System;
using System.Collections.Generic;
using System.Text;

namespace Models.DTOs
{
    public class AnimalBookingDto
    {
        public string? AppointmentId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string? Address { get; set; }
    }
}
