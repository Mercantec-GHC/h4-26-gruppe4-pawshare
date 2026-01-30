namespace Models
{
    public class Appointment : Common
    {
        public required DateTime Start {  get; set; }
        public required DateTime End { get; set; }
        public required string Address { get; set; }
        public required string Description { get; set; }
        public List<UserAppointmentBooking>? Users { get; set; }
        public required List<AppointmentAnimalBooking> Animals { get; set; }

    }
}
