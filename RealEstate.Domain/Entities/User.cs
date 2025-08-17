using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Domain.Entities
{
    public class User: IdentityUser
    {
        [MaxLength(50)]
        public required string FirstName { get; set; }

        [MaxLength(50)]
        public required string LastName { get; set; }

        public bool Active { get; set; }
    }
}
