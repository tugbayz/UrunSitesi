using Microsoft.AspNetCore.Mvc.Rendering; 

namespace PackProApp.Areas.Admin.Modelss.CategoryVMs
{
    public class AdminCategoryCreateVM
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }

        // Alt kategori ekleme için mevcut kategoriler listesi
        public Guid? ParentCategoryId { get; set; } // Seçilen üst kategori ID'si
        public SelectList? ParentCategories { get; set; } // Üst kategorilerin listesi

        // Yeni alt kategoriler için 
        public List<AdminSubCategoryCreateVM> Subcategories { get; set; } = new List<AdminSubCategoryCreateVM>();
    }
}
