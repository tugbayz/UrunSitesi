namespace PackProApp.DTOs.SubCategoryDTOs
{

    public class SubCategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Kategori ile ilişki için CategoryId
        public Guid CategoryId { get; set; }

        // Ana kategori adı (isteğe bağlı)
        public string CategoryName { get; set; }
    }



}
