using PackProApp.Entities;
using System.ComponentModel;

namespace PackProApp.Areas.Admin.Modelss.ProductVMs
{
    public class AdminProductListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        public byte[]? Image { get; set; }
        public string? ImageBase64 { get; set; } // Base64 formatı için yeni bir özellik
    }
}
