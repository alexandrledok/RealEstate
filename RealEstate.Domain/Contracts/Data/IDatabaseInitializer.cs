namespace RealEstate.Domain.Contracts.Data
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }
}
