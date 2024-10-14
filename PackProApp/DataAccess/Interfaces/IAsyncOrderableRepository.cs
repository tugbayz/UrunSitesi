using PackProApp.Core.BaseEntities;
using System.Linq.Expressions;

namespace PackProApp.DataAccess.Interfaces
{
    public interface IAsyncOrderableRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, TKey>> orderBy, bool orderBySDesc, bool tracking = true);

        Task<IEnumerable<TEntity>> GetAllAsync<TKey>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TKey>> orderBy, bool orderBySDesc, bool tracking = true);

    }
}
