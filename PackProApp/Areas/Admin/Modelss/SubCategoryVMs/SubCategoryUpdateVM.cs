using Microsoft.AspNetCore.Mvc.Rendering;

namespace PackProApp.Areas.Admin.Models.SubCategoryVMs
{
    public class SubCategoryUpdateVM
    {
        public Guid Id { get; set; } // Güncellenen alt kategorinin ID'si
        public string Name { get; set; }
        public string Description { get; set; }

        // Kategori seçimi için kullanacağımız SelectList
        public Guid CategoryId { get; set; }
        public SelectList Categories { get; set; }
    }
}
