using PackProApp.DTOs.ProductDTOs;
using PackProApp.DTOs.SubCategoryDTOs;

namespace PackProApp.DTOs.CategoryDTOs
{
    public class CategoryUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }

        // Alt kategorilerin güncellenmesi için liste
        public List<SubCategoryUpdateDTO> SubCategories { get; set; } = new List<SubCategoryUpdateDTO>();
    }
}
