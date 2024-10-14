using PackProApp.DTOs.ProductDTOs;
using PackProApp.DTOs.SubCategoryDTOs;

namespace PackProApp.DTOs.CategoryDTOs
{
    public class CategoryCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; } // Üst kategori ID'si (isteğe bağlı)

        // Yeni alt kategoriler
        public List<SubCategoryCreateDTO> Subcategories { get; set; } = new List<SubCategoryCreateDTO>();
    }

  

}
