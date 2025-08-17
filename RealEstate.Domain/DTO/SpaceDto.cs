namespace RealEstate.Domain.DTO
{
    public class SpaceDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string Type { get; set; }
        public float Size { get; set; }
        public string? Description { get; set; }
        public PropertyDto Property { get; set; }
    }

}
