namespace Models
{
    public class Animal : Common
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Base64Image { get; set; }
        public required int Age { get; set; }
        public required string TypeId { get; set; }
        public AnimalType? AnimalType { get; set; }
        public required string UserId { get; set; }
        public User? User { get; set; }
        public List<AppointmentAnimalBooking>? Bookings { get; set; }
    }
}
