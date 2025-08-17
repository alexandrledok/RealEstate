using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Domain.Contracts.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstate.Domain.Contracts.Data
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppSettings _appSettings;
        public AccountService(
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager,
            IOptions<AppSettings> config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appSettings = config.Value;
        }


        public async Task<string> CreateRolesAsync(IdentityRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("\r\n", result.Errors));

            role = await _roleManager.FindByNameAsync(role.Name);
 
            return role.Id;
        }

        public async Task<IEnumerable<IdentityRole>> GetRolesAsync()
        {
            return await Task.FromResult(_roleManager.Roles.ToArray());
        }


        public async Task<string> CreateUserAsync(User user, string[] roles, string password)
        {
            var ddUser = await _userManager.FindByNameAsync(user.UserName);
            if (ddUser != null)
                throw new UserAlreadyRegisteredException($"User with username {user.UserName} already exists");

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("\r\n", result.Errors));

            user = await _userManager.FindByNameAsync(user.UserName);
            result = await _userManager.AddToRolesAsync(user, roles.Distinct());
       
            if (!result.Succeeded)
                throw new InvalidOperationException(string.Join("\r\n", result.Errors));

            return user.Id;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await Task.FromResult(_userManager.Users.ToArray());
        }

        public async Task<string> SignInAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user,password))
                throw new UnauthorizedAccessException("Invalid credentials");

            var jwtKey = _appSettings.Jwt.Key;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, string.Join(",", await _userManager.GetRolesAsync(user)))
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
    }
}
