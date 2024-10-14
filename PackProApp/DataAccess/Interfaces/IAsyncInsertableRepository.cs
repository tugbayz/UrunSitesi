using PackProApp.Core.BaseEntities;

namespace PackProApp.DataAccess.Interfaces
{
    public interface IAsyncInsertableRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}
