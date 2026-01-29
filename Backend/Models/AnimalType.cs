namespace Models
{
    public class AnimalType : Common
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public List<Animal>? Animals { get; set; }
    }
}
