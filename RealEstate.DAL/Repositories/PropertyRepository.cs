using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Contracts.Data;
using RealEstate.Domain.Entities;

namespace RealEstate.DAL.Repositories
{
    public class PropertyRepository : Repository<Property>, IPropertyRepository
    {
        public PropertyRepository(RealEstateContext dbContext) : base(dbContext)
        {
        }

        public async Task<Property> AddAsync(Property property)
        {
            await _context.Properties.AddAsync(property);
            return property;
        }

        public async Task<IEnumerable<Property>> GetByFilters(string type = null, float? minPrice = null, float? maxPrice = null, int page = 1, int limit = 10, string sort = "asc")
        {
            var query = _context.Properties.Include(p => p.Spaces).AsQueryable();

            if (!string.IsNullOrEmpty(type))
                query = query.Where(p => p.Type == type);
            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice);
            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice);

            query = sort == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);

            return await query.Skip((page - 1) * limit).Take(limit).ToListAsync();
        }
    }

}
