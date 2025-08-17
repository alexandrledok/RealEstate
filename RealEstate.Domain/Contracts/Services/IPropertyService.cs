using RealEstate.Domain.DTO;

namespace RealEstate.Domain.Contracts.Services
{
    public interface IPropertyService 
    {
        Task<int> CreateAsync(PropertyDto propertyDto);
        Task<IEnumerable<PropertyDto>> GetByFilters(string type = null, float? minPrice = null, float? maxPrice = null, int page = 1, int limit = 10, string sort = "asc");
        Task<PropertyDto> GetByIdAsync(int id);
    }

}
