using PackProApp.DataAccess.Interfaces;
using PackProApp.Entities;

namespace PackProApp.Repositories.CategoryRepositories
{
    public interface ICategoryRepository: IRepository, IAsyncDeletableRepository<Category>, IAsyncFindableRepository<Category>, IAsyncInsertableRepository<Category>, IAsyncOrderableRepository<Category>, IAsyncQueryableRepository<Category>, IAsyncRepository, IAsyncTransactionRepository, IAsyncUpdatableRepository<Category>
    {
    }
}
