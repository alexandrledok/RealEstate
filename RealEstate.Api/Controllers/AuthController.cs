using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Api.ViewModels;
using RealEstate.Api.ViewModels.User;
using RealEstate.Domain.Contracts.Services;

namespace RealEstate.Api.Controllers
{
    // Home Assignment: RULE
    // Example controller to get simple jwt token 
    // for Home Assignment task and requrements for simple Auth
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponce), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest model)
        {
            var token = await _accountService.SignInAsync(model.Email, model.Password);

            return Ok(new LoginResponce { Token = token });
        }
    }
}
