using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Contracts.Data;
using RealEstate.Domain.Entities;

namespace RealEstate.DAL.Repositories
{
    public class SpaceRepository : Repository<Space>, ISpaceRepository
    {
        public SpaceRepository(RealEstateContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Space>> AddAsync(IEnumerable<Space> spaces)
        {
            await _context.Spaces.AddRangeAsync(spaces);
            return spaces;
        }

        public async Task<IEnumerable<Space>> GetByPropertyIdAsync(int propertyId, string type = null, float? minSize = null)
        {
            var query = _context.Spaces.Where(s => s.PropertyId == propertyId);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(s => s.Type == type);
            if (minSize.HasValue)
                query = query.Where(s => s.Size >= minSize);

            return await query.ToListAsync();
        }
    }
}
