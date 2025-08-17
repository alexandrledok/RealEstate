using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.ViewModels;
using RealEstate.Api.ViewModels.Spaces;
using RealEstate.Domain.Contracts.Services;
using RealEstate.Domain.Exceptions;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/v1/spaces")]
    public class SpacesController : ControllerBase
    {
        private readonly ISpaceService _spaceService;
        private readonly IMapper _mapper;

        public SpacesController(ISpaceService spaceService, IMapper mapper)
        {
            _spaceService = spaceService;
            _mapper= mapper;
        }

        // Home Assignment: RULE
        // o GET /spaces: List spaces with filters:
        //    	Query params: property_id(required), type(e.g., ? type = bedroom), min_size.
        //     Demonstrate efficient querying of related data.
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SpaceVM>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByPropertyId([FromQuery] int propertyId, [FromQuery] string? type, [FromQuery] float? minSize)
        {
            //Exception handling it's Home Assignment Rule 
            if (propertyId <= 0)
                throw new RequestArgumentException($"Property id: {propertyId} is invalid");
            //Exception handling it's Home Assignment Rule 
            if (minSize.HasValue && minSize <= 0)
                throw new RequestArgumentException("Invalid minSize value");


            var spaces = await _spaceService.GetByPropertyIdAsync(propertyId, type, minSize);
            var result = _mapper.Map<IEnumerable<SpaceVM>>(spaces);
            return Ok(result);
        }
    }

}
