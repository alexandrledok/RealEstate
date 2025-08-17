using RealEstate.Domain.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.DTO;
using RealEstate.Domain.Contracts.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Predefined;
using Microsoft.AspNetCore.Identity;

namespace RealEstate.DAL
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly IAccountService _accountService;
        private readonly IPropertyService _propertyService;
        private readonly RealEstateContext _context;

        public DatabaseInitializer(RealEstateContext context,
            IAccountService accountService,
            IPropertyService propertyService)
        {
            _context = context;

            _accountService = accountService;
            _propertyService = propertyService;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();
            await SeedExampleData();
        }

        private async Task SeedExampleData()
        {
            await EnsureProperties();

            await EnsureRoles();

            await EnsureUsers();
        }

        private async Task EnsureUsers()
        {
            IEnumerable<User> users = await _accountService.GetUsersAsync();
            if (users.Count() == 0)
            {
                await CreateUserAsync("Admin", "Admin", "Pass4Admin!", "admin@gmail.com", "+1 (123) 000-0000", new[] { RoleConstants.Admin });
                await CreateUserAsync("User", "Test", "Pass4User!", "user@gmail.com", "+1 (123) 000-0001", new[] { RoleConstants.User });
            }
        }

        private async Task EnsureRoles()
        {
            IEnumerable<IdentityRole> roles = await _accountService.GetRolesAsync();
            if (roles.Count() == 0)
            {
                await CreateRoleAsync(RoleConstants.Admin);
                await CreateRoleAsync(RoleConstants.User);
            }
        }

        private async Task EnsureProperties()
        {
            var props = await _propertyService.GetByFilters();
            if (props.Count() == 0)
            {
                var properties = new List<PropertyDto>
               {
                   new PropertyDto
                   {
                       Type = "Apartment",
                       Price = 150000,
                       Description = "A cozy apartment in the city center.",
                       Address = "123 Main St, Cityville",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Room", Size = 30 },
                           new SpaceDto { Type = "Bedroom", Size = 20 },
                           new SpaceDto { Type = "Kitchen", Size = 15 }
                       }
                   },
                   new PropertyDto
                   {
                       Type = "House",
                       Price = 300000,
                       Description = "A spacious house with a garden.",
                       Address = "456 Elm St, Suburbia",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Room", Size = 40 },
                           new SpaceDto { Type = "Bedroom", Size = 25 },
                           new SpaceDto { Type = "Kitchen", Size = 20 },
                           new SpaceDto { Type = "Garage", Size = 50 }
                       }
                   },
                   new PropertyDto
                   {
                       Type = "Condo",
                       Price = 200000,
                       Description = "A modern condo with great amenities.",
                       Address = "789 Oak St, Metropolis",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Room", Size = 35 },
                           new SpaceDto { Type = "Bedroom", Size = 22 },
                           new SpaceDto { Type = "Kitchen", Size = 18 }
                       }
                   },
                   new PropertyDto
                   {
                       Type = "Villa",
                       Price = 500000,
                       Description = "A luxurious villa with a pool.",
                       Address = "101 Palm St, Beachside",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Room", Size = 50 },
                           new SpaceDto { Type = "Bedroom", Size = 30 },
                           new SpaceDto { Type = "Kitchen", Size = 25 },
                           new SpaceDto { Type = "Pool Area", Size = 100 }
                       }
                   },
                   new PropertyDto
                   {
                       Type = "Townhouse",
                       Price = 250000,
                       Description = "A charming townhouse in a quiet neighborhood.",
                       Address = "202 Maple St, Townsville",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Room", Size = 32 },
                           new SpaceDto { Type = "Bedroom", Size = 24 },
                           new SpaceDto { Type = "Kitchen", Size = 20 }
                       }
                   },
                   new PropertyDto
                   {
                       Type = "Penthouse",
                       Price = 800000,
                       Description = "A stunning penthouse with a city view.",
                       Address = "303 Skyline Ave, Downtown",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Room", Size = 60 },
                           new SpaceDto { Type = "Bedroom", Size = 40 },
                           new SpaceDto { Type = "Kitchen", Size = 30 },
                           new SpaceDto { Type = "Terrace", Size = 120 }
                       }
                   },
                   new PropertyDto
                   {
                       Type = "Cottage",
                       Price = 180000,
                       Description = "A cozy cottage in the countryside.",
                       Address = "404 Forest Rd, Countryside",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Room", Size = 28 },
                           new SpaceDto { Type = "Bedroom", Size = 18 },
                           new SpaceDto { Type = "Kitchen", Size = 15 }
                       }
                   },
                   new PropertyDto
                   {
                       Type = "Studio",
                       Price = 120000,
                       Description = "A compact studio apartment.",
                       Address = "505 Compact Ln, Cityville",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Area", Size = 25 },
                           new SpaceDto { Type = "Kitchenette", Size = 10 }
                       }
                   },
                   new PropertyDto
                   {
                       Type = "Farmhouse",
                       Price = 400000,
                       Description = "A farmhouse with vast land.",
                       Address = "606 Country Rd, Ruralville",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Room", Size = 45 },
                           new SpaceDto { Type = "Bedroom", Size = 35 },
                           new SpaceDto { Type = "Kitchen", Size = 25 },
                           new SpaceDto { Type = "Barn", Size = 200 }
                       }
                   },
                   new PropertyDto
                   {
                       Type = "Loft",
                       Price = 220000,
                       Description = "A trendy loft in the arts district.",
                       Address = "707 Art St, Creative City",
                       Spaces = new List<SpaceDto>
                       {
                           new SpaceDto { Type = "Living Area", Size = 50 },
                           new SpaceDto { Type = "Bedroom", Size = 30 },
                           new SpaceDto { Type = "Kitchen", Size = 20 }
                       }
                   }
               };

                foreach (var property in properties)
                {
                    await _propertyService.CreateAsync(property);
                }
            }
        }

        private async Task<string> CreateRoleAsync(string roleName)
        {
            IdentityRole role = new IdentityRole
            {
                Name = roleName,
            };

            var roleId = await _accountService.CreateRolesAsync(role);

            role.Id = roleId;
            return role.Id;
        }

        private async Task<User> CreateUserAsync(string firstName, string lastName, string password, string email,
           string phoneNumber, string[] roles)
        {
            User user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = email,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                Active = true
            };

            var userId = await _accountService.CreateUserAsync(user, roles, password);

            user.Id = userId;
            return user;
        }
    }
}
