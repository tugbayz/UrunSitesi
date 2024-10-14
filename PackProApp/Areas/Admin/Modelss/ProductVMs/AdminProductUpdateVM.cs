using Microsoft.AspNetCore.Mvc.Rendering;

namespace PackProApp.Areas.Admin.Modelss.ProductVMs
{
    public class AdminProductUpdateVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Ürünün atanacağı kategori ID'si
        public Guid CategoryId { get; set; }

        // Kategori seçimi için SelectList
        public SelectList? Categories { get; set; }
    }
}
