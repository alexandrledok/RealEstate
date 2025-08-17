using RealEstate.Domain.DTO;

namespace RealEstate.Domain.Contracts.Services
{
    public interface ISpaceService
    {
        Task<IEnumerable<SpaceDto>> GetByPropertyIdAsync(int propertyId, string? type, float? min_size);
    }
}
