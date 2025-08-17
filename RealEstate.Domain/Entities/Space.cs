namespace RealEstate.Domain.Entities
{
    public class Space: BaseEntity
    {
        public int PropertyId { get; set; }
        public required string Type { get; set; }
        public float Size { get; set; }
        public string? Description { get; set; }

        public required Property Property { get; set; }
    }

}
