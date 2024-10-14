using PackProApp.Core.BaseEntities;

namespace PackProApp.Entities
{
    public class Product : AuditableEntity
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public byte[]? Image { get; set; }

        //// Ürün bir alt kategoriye ait olabilir.
        //public Guid? SubCategoryId { get; set; } // Ürün sadece bir alt kategoriye ait olacak
        //public virtual SubCategory SubCategory { get; set; } // Alt kategori referansı

        // Eğer istenirse, sadece bir ana kategoriye ait olabilir.
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; } // Kategori referansı
    }
}
