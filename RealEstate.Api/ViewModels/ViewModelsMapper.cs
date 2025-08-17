using AutoMapper;
using RealEstate.Api.ViewModels.Properties;
using RealEstate.Api.ViewModels.Spaces;
using RealEstate.Api.ViewModels.Stats;
using RealEstate.Domain.DTO;

namespace RealEstate.Api.ViewModels
{
    public class ViewModelsMapper : Profile
    {
        public ViewModelsMapper()
        {
            MapModels();
        }

        public void MapModels()
        {
            CreateMap<PropertyVM, PropertyDto>()
                .ReverseMap();
            CreateMap<SpaceVM, SpaceDto>()
               .ReverseMap();
            CreateMap<StatsVm, StatsDto>()
               .ReverseMap();
        }
    }
}
