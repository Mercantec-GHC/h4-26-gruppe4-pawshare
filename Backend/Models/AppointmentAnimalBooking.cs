using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class AppointmentAnimalBooking : Common
    {
        [Required(ErrorMessage = "Der skal være mindst en tilkoblet aftale")]
        public required int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        [Required(ErrorMessage = "Der skal være mindst et tilkoblet dyr")]
        public required int AnimalId { get; set; }
        public Animal? Animal { get; set; }
    }
}
