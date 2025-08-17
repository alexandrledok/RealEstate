using RealEstate.DAL.Repositories;
using RealEstate.Domain.Contracts.Data;

namespace RealEstate.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RealEstateContext _context;
        public IPropertyRepository Properties { get; private set; }
        public ISpaceRepository Spaces { get; private set; }

        public UnitOfWork(RealEstateContext context)
        {
            _context = context;
            Properties = new PropertyRepository(_context);
            Spaces = new SpaceRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
