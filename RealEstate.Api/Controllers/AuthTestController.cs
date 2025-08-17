using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.ViewModels;

namespace RealEstate.Api.Controllers
{
    // Home Assignment: RULE
    // o	Optional (for extra credit): Basic auth, rate limiting, or Dockerization.
    [Route("api/v1/auth-test")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult PublicEndpoint() => Ok("This endpoint is public.");

        [HttpGet("private")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        public IActionResult PrivateEndpoint() => Ok($"Hello, {User?.Identity?.Name}!");
    }
}
