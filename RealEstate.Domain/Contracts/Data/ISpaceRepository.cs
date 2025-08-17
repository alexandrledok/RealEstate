using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Contracts.Data
{
    public interface ISpaceRepository : IRepository<Space>
    {
        Task<IEnumerable<Space>> AddAsync(IEnumerable<Space> spaces);
        Task<IEnumerable<Space>> GetByPropertyIdAsync(int propertyId, string? type = null, float? minSize = null);
    }
}
