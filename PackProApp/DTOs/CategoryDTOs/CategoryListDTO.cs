namespace PackProApp.DTOs.CategoryDTOs
{
    public class CategoryListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }

        // Alt kategorileri tutacak liste
        public List<CategoryListDTO> SubCategories { get; set; } = new List<CategoryListDTO>();
    }
}
