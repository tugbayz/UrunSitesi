using PackProApp.DTOs.ProductDTOs;

namespace PackProApp.DTOs.CategoryDTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }

        // Alt ve üst kategori ilişkilerini göstermek için eklemeler
        public Guid? ParentCategoryId { get; set; } // Üst kategori olabilir
        public string ParentCategoryName { get; set; } // Üst kategorinin adı
        public List<CategoryDTO> SubCategories { get; set; } = new List<CategoryDTO>(); // Alt kategoriler

        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
