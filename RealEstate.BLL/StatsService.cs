using RealEstate.Domain.Contracts.Data;
using RealEstate.Domain.Contracts.Services;
using RealEstate.Domain.DTO;
using Microsoft.EntityFrameworkCore;

namespace RealEstate.BLL
{
    public class StatsService : IStatsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<StatsDto>> GetStatsAsync()
        {
            var query = _unitOfWork.Properties.GetAll()
                .SelectMany(p => p.Spaces, (p, s) => new { p.Type, s.Size })
                .GroupBy(x => x.Type)
                .Select(g => new StatsDto
                {
                    PropertyType = g.Key,
                    AvgSpaceSize = g.Average(x => x.Size)
                });
            var stats = await query.ToListAsync();

            return stats;

        }
    }
}
