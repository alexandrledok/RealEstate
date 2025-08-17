using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.ViewModels;
using RealEstate.Api.ViewModels.Properties;
using RealEstate.Domain.Contracts.Services;
using RealEstate.Domain.DTO;
using RealEstate.Domain.Exceptions;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/v1/properties")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public PropertiesController(IPropertyService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }

        // Home Assignment: RULE
        // o GET /properties: List all properties with optional filters:
        //  	Query params: type(e.g., ? type = house), min_price and max_price(e.g., ? min_price = 100000 & max_price = 500000).
        //  	Response: JSON array of properties, each including a nested array of associated spaces.
        // o Implement a GET endpoint to retrieve all properties with optional filters for type, min_price, and max_price.
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PropertyVM>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAll([FromQuery] string? type, [FromQuery] float? minPrice, [FromQuery] float? maxPrice, 
            [FromQuery] int page = 1, [FromQuery] int limit = 10, 
            [FromQuery] string sort = "asc")
        {
            var properties = await _propertyService.GetByFilters(type, minPrice, maxPrice, page, limit, sort);
            var vm = _mapper.Map<IEnumerable<PropertyVM>>(properties);
            return Ok(vm);
        }

        //// Home Assignment: RULE
        //o GET /properties/:id: Retrieve a single property by ID, including its spaces.
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PropertyVM), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<PropertyVM>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetById(int id)
        {
            //Exception handling it's Home Assignment Rule 
            if (id <= 0)
                throw new RequestArgumentException($"Property id: {id} is invalid");

            var property = await _propertyService.GetByIdAsync(id);

            //Exception handling it's Home Assignment Rule 
            if (property == null)
                throw new NotFoundException($"Property with id: {id} not found");

            var vm = _mapper.Map<PropertyVM>(property);
            return Ok(vm);
        }

        // Home Assignment: RULE
        //  o   POST /properties: Create a new property.
        //  	Accept JSON body with Property fields(except id); optionally include a nested array for spaces.
        //     Validate inputs(e.g., price > 0, type from allowed values).
        //          I using model state for example and exception handling (see ExceptionHandlerMiddleware.cs)
        //          because its rule of Home Assignment
        [HttpPost]
        [ProducesResponseType(typeof(PropertyVM), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<PropertyVM>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] PropertyVM property)
        {
            if (!ModelState.IsValid)
                throw new RequestArgumentException(ModelState);

            var dto = _mapper.Map<PropertyDto>(property);
            property.Id = await _propertyService.CreateAsync(dto);

            return Ok(property); 
        }
    }

}
