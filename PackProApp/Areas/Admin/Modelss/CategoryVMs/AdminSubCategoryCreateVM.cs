using Microsoft.AspNetCore.Mvc.Rendering;

namespace PackProApp.Areas.Admin.Modelss.CategoryVMs
{
    public class AdminSubCategoryCreateVM
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
    }
}
