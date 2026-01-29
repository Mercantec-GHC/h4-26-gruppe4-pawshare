using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class UserAppointmentBooking : Common
    {
        [Required(ErrorMessage = "Der skal være mindst en tilkoblet bruger")]
        public required int UserId { get; set; }
        public User? User { get; set; }

        [Required(ErrorMessage = "Der skal være mindst en tilkoblet aftale")]
        public required int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
    }
}
