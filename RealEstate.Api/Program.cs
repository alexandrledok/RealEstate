
namespace RealEstate.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder
                .BuildServices()
                .Build()
                .BuildPipleline()
                // Take-Home Assignment: 
                //•	Seed the database with at least 5 - 10 sample properties, each with 2 - 5 spaces, to demonstrate functionality.
                // Apply migrations and base DB seeding if Database is empty 
                .SeedDatabase()
                .Run();
        }
    }
}
