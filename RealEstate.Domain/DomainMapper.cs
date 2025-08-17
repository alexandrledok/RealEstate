using AutoMapper;
using RealEstate.Domain.DTO;
using RealEstate.Domain.Entities;

namespace RealEstate.Domain
{
    public class DomainMapper : Profile
    {
        public DomainMapper()
        {
            MapModels();
        }

        public void MapModels()
        {
            CreateMap<Property, PropertyDto>()
                .ReverseMap();
            CreateMap<Space, SpaceDto>()
               .ReverseMap();
            CreateMap<User, UserDto>()
               .ReverseMap();
        }
    }
}
