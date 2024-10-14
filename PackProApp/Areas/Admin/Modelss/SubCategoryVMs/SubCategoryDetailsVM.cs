namespace PackProApp.Areas.Admin.Models.SubCategoryVMs
{
    public class SubCategoryDetailsVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Kategori bilgisi de göstermek için
        public string CategoryName { get; set; }
    }
}
