namespace RealEstate.Domain
{

    // Take-Home Assignment: 
    //o Use environment variables for config(e.g., DB connection) 
    public class AppSettings
    {
        public required DbConnections ConnectionStrings { get; set; }
        public required string AllowedHosts { get; set; }
        public required JwtSettings Jwt { get; set; }
    }

    public class DbConnections { 
        public required string DefaultConnection { get; set; }
    } 

    public class JwtSettings
    {
        public required string Key { get; set; }
        public required int ExpirationMinutes { get; set; }
    }
}
