namespace Models
{
    public class Animal : Common
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int age { get; set; }
        public required string TypeId { get; set; }
        public AnimalType? AnimalType { get; set; }
        public required string UserId { get; set; }
        public User? user { get; set; }
    }
}
