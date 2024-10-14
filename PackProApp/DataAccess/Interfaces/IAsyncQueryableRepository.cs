using PackProApp.Core.BaseEntities;
using System.Linq.Expressions;

namespace PackProApp.DataAccess.Interfaces
{
    public interface IAsyncQueryableRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> Expression, bool tracking = true);
    }
}
