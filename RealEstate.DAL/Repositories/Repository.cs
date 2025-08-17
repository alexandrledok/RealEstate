using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Contracts.Data;
using RealEstate.Domain.Entities;

namespace RealEstate.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
         where TEntity : BaseEntity
    {
        protected RealEstateContext _context;
        public Repository(RealEstateContext dbContext)
        {
            this._context = dbContext;
        }


        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToArrayAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>()
                .FirstOrDefaultAsync(el => el.Id == id);
        }
    }

}
