namespace RealEstate.Domain.DTO
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Type { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public List<SpaceDto> Spaces { get; set; } = new();
    }
}
