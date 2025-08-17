using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.ViewModels;
using RealEstate.Api.ViewModels.Stats;
using RealEstate.Domain.Contracts.Services;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/v1/stats")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService _statsService;
        private readonly IMapper _mapper;

        public StatsController(IStatsService statsService, IMapper mapper)
        {
            _statsService = statsService;
            _mapper= mapper;
        }

        // Home Assignment: RULE
        // o AddAsync one aggregated query endpoint, e.g.,
        // GET /stats: Return stats like average space size per property type, to demonstrate complex querying.
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StatsVm>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            var stats = await _statsService.GetStatsAsync();
            var result = _mapper.Map<IEnumerable<StatsVm>>(stats);
            return Ok(stats);
        }
    }

}
