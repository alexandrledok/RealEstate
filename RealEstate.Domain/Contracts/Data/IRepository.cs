namespace RealEstate.Domain.Contracts.Data
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
    }

}
