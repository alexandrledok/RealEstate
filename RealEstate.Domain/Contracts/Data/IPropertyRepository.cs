using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Contracts.Data
{
    public interface IPropertyRepository : IRepository<Property>
    {
        Task<Property> AddAsync(Property property);
        Task<IEnumerable<Property>> GetByFilters(string type = null, float? minPrice = null, float? maxPrice = null, int page = 1, int limit = 10, string sort = "asc");
    }

}
