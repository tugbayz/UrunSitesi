namespace PackProApp.DTOs.SubCategoryDTOs
{
    public class SubCategoryUpdateDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Ana kategori ile ilişkilendirme
        public Guid CategoryId { get; set; }
    }



}
