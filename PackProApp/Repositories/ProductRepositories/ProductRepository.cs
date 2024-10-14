using PackProApp.AppContext;
using PackProApp.DataAccess.EntityFramework;
using PackProApp.Entities;

namespace PackProApp.Repositories.ProductRepositories
{
    public class ProductRepository: EFBaseRepository<Product>,IProductRepository
    {
        public ProductRepository(AppDbContext context):base(context)
        {
        }
    }
}
