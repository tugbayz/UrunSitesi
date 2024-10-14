using Microsoft.AspNetCore.Mvc.Rendering;
using PackProApp.Areas.Admin.Modelss.ProductVMs;

namespace PackProApp.Areas.Admin.Modelss.CategoryVMs
{
    public class AdminCategoryUpdateVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }

        // Ana kategorileri seçmek için kullanacağımız SelectList
        public SelectList ParentCategories { get; set; }

        // Alt kategorileri tutan liste
        public List<SubCategoryVM> SubCategories { get; set; } = new List<SubCategoryVM>();
    }
    public class SubCategoryVM
    {
        public Guid Id { get; set; } // SubCategory'ye özgü ID
        public string Name { get; set; }
        public string Description { get; set; }
    }

}
