using RealEstate.Api.ViewModels.Spaces;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Api.ViewModels.Properties
{
    public class PropertyVM
    {
        public int Id { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Type { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public float Price { get; set; }
        public string Description { get; set; }
        public List<SpaceVM> Spaces { get; set; } = new();
    }
}
