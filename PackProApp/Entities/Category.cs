using PackProApp.Core.BaseEntities;

namespace PackProApp.Entities
{
    public class Category : AuditableEntity
    {
        public Category()
        {
            Products = new HashSet<Product>();
            //SubCategories = new HashSet<SubCategory>(); // Alt kategoriler için doğru koleksiyon tipi
        }

        public string Name { get; set; }
        public string Description { get; set; }

        public Guid? ParentCategoryId { get; set; } // Üst kategori referansı
        public virtual Category ParentCategory { get; set; } // Üst kategori
        public virtual ICollection<Category> SubCategories { get; set; } // Alt kategoriler
        public virtual ICollection<Product> Products { get; set; } // Kategoriye ait ürünler
    }
}
