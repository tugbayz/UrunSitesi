using PackProApp.DataAccess.Interfaces;
using PackProApp.Entities;

namespace PackProApp.Repositories.CustomerRepositories
{
    public interface ICustomerRepository : IRepository, IAsyncDeletableRepository<Customer>, IAsyncFindableRepository<Customer>, IAsyncInsertableRepository<Customer>, IAsyncOrderableRepository<Customer>, IAsyncQueryableRepository<Customer>, IAsyncRepository, IAsyncTransactionRepository, IAsyncUpdatableRepository<Customer>
    {
    }
}
