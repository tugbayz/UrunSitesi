namespace PackProApp.DTOs.SubCategoryDTOs
{
    public class SubCategoryListDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CategoryName { get; set; } // Bağlı olduğu kategori adı
    }

}
