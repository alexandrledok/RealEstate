using AutoMapper;
using RealEstate.Domain.Contracts.Data;
using RealEstate.Domain.Contracts.Services;
using RealEstate.Domain.DTO;
using RealEstate.Domain.Entities;

namespace RealEstate.BLL
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWor;
        private readonly IMapper _mapper;

        public PropertyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWor = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyDto>> GetByFilters(string type = null, float? minPrice = null, float? maxPrice = null, int page = 1, int limit = 10, string sort = "asc")
        {
            var properties = await _unitOfWor.Properties.GetByFilters(type, minPrice, maxPrice, page, limit, sort);

            // I using mapper 
            var result = _mapper.Map<IEnumerable<PropertyDto>>(properties);
            // But we also can to map Property entities to PropertyDto like below (a bit faster but a bit more coce)
            // so i just show 2 ways to do it
            //var result = properties.Select(property => new PropertyDto
            //{
            //    Id = property.Id,
            //    Address = property.Address,
            //    Type = property.Type,
            //    Price = property.Price,
            //    Description = property.Description,
            //    Spaces = property.Spaces.Select(space => new SpaceDto
            //    {
            //        Id = space.Id,
            //        Size = space.Size,
            //        Type = space.Type
            //    }).ToList()
            //});

            return result;
        }

        public async Task<int> CreateAsync(PropertyDto property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            Property dbProperty = _mapper.Map<Property>(property);

            await _unitOfWor.Properties.AddAsync(dbProperty);
            //await _unitOfWor.Spaces.AddAsync(dbProperty.Spaces);
            await _unitOfWor.CompleteAsync();

            return dbProperty.Id;
        }

        public async Task<PropertyDto> GetByIdAsync(int id)
        {
            var property = await _unitOfWor.Properties.GetByIdAsync(id);
            if (property == null)
                return null;

            var result = _mapper.Map<PropertyDto>(property);
            return result;
        }
    }

}
