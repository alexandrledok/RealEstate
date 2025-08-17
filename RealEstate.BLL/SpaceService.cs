using AutoMapper;
using RealEstate.Domain.Contracts.Data;
using RealEstate.Domain.Contracts.Services;
using RealEstate.Domain.DTO;

namespace RealEstate.BLL
{
    public class SpaceService : ISpaceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpaceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SpaceDto>> GetByPropertyIdAsync(int propertyId, string? type, float? minSize)
        {

            var spaces = await _unitOfWork.Spaces.GetByPropertyIdAsync(propertyId, type, minSize);
            var result = _mapper.Map<IEnumerable<SpaceDto>>(spaces);
            return result;
        }
    }
}
