using Microsoft.AspNetCore.Identity;
using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Contracts.Services
{
    public interface IAccountService
    {
        Task<string> CreateRolesAsync(IdentityRole role);
        Task<IEnumerable<IdentityRole>> GetRolesAsync();

        Task<string> CreateUserAsync(User user, string[] roles, string password);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<string> SignInAsync(string username, string password);
    }
}
