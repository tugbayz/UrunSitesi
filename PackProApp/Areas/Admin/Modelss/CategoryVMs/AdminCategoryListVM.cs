namespace PackProApp.Areas.Admin.Modelss.CategoryVMs
{
    public class AdminCategoryListVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }

        // Alt kategori ilişkisini göstermek için
        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }

        public List<AdminCategoryListVM> SubCategories { get; set; } = new List<AdminCategoryListVM>();
    }
}
