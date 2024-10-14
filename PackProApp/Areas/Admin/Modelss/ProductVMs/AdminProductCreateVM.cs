using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PackProApp.Areas.Admin.Modelss.ProductVMs
{
    public class AdminProductCreateVM
    {
        [Required]
        public string? Name { get; set; }
        public decimal Price { get; set; }

        // Ürünün atanacağı kategori ID'si
        public Guid CategoryId { get; set; }

        // Kategori seçimi için SelectList
        public SelectList? Categories { get; set; }

        public string? Image { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
