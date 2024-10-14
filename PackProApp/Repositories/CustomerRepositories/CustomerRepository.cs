using PackProApp.AppContext;
using PackProApp.DataAccess.EntityFramework;
using PackProApp.Entities;
using PackProApp.Repositories.ProductRepositories;

namespace PackProApp.Repositories.CustomerRepositories
{
    public class CustomerRepository : EFBaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

    }
}
