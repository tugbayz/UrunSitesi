using PackProApp.DataAccess.Interfaces;
using PackProApp.Entities;

namespace PackProApp.Repositories.ProductRepositories
{
    public interface IProductRepository:IRepository,IAsyncDeletableRepository<Product>,IAsyncFindableRepository<Product>,IAsyncInsertableRepository<Product>,IAsyncOrderableRepository<Product>,IAsyncQueryableRepository<Product>, IAsyncRepository,IAsyncTransactionRepository,IAsyncUpdatableRepository<Product>
    {
    }
}
