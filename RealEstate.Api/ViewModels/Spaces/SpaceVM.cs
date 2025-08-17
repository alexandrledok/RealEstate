namespace RealEstate.Api.ViewModels.Spaces
{
    public class SpaceVM
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string Type { get; set; }
        public float Size { get; set; }
        public string? Description { get; set; }
    }

}
