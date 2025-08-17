namespace RealEstate.Domain.Contracts.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IPropertyRepository Properties { get; }
        ISpaceRepository Spaces { get; }
        Task<int> CompleteAsync();
    }
}
