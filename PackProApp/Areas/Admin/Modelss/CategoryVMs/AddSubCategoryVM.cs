

using Microsoft.AspNetCore.Mvc.Rendering;

namespace PackProApp.Areas.Admin.Modelss.CategoryVMs
{
    public class AddSubCategoryVM
    {
        public Guid? SelectedCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryDescription { get; set; }
        public SelectList Categories { get; set; } // Ana kategoriler
    }
}

