namespace RealEstate.Domain.DTO
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // public TokenDto AppToken { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool Active { get; set; }
        public bool EmailConfirmed { get; set; }
        public string UserName { get; set; }
        public bool IsEnabled { get; set; }
        public string IdNumber { get; set; }
        public string Role { get; set; }
    }
}
