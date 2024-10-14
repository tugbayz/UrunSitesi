using Microsoft.AspNetCore.Mvc.Rendering;

namespace PackProApp.Areas.Admin.Models.SubCategoryVMs
{
    public class SubCategoryCreateVM
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Kategori seçimi için kullanacağımız SelectList
        public Guid CategoryId { get; set; }
        public SelectList Categories { get; set; }
    }
}
