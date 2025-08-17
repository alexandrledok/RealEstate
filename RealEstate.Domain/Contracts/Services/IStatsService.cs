using RealEstate.Domain.DTO;

namespace RealEstate.Domain.Contracts.Services
{
    public interface IStatsService
    {
        Task<IEnumerable<StatsDto>> GetStatsAsync();
    }
}
