namespace RealEstate.Domain.Entities
{
    public class Property: BaseEntity
    {
        public required string Address { get; set; }
        public required string Type { get; set; }
        public required float Price { get; set; }
        public string? Description { get; set; }
        public List<Space> Spaces { get; set; } = new();
    }
}
