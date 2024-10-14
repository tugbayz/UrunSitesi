using PackProApp.Core.BaseEntities;

namespace PackProApp.DataAccess.Interfaces
{
    public interface IAsyncUpdatableRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}
