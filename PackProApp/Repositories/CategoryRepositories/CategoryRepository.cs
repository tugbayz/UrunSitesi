using PackProApp.AppContext;
using PackProApp.DataAccess.EntityFramework;
using PackProApp.Entities;

namespace PackProApp.Repositories.CategoryRepositories
{
    public class CategoryRepository : EFBaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
