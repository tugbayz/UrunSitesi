namespace PackProApp.Areas.Admin.Modelss.CategoryVMs
{
    public class AdminCategoryDetailsVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid? ParentCategoryId { get; set; }
        public string ParentCategoryName { get; set; }

        // Alt kategorileri tutan liste
        public List<SubCategoryVM> SubCategories { get; set; } = new List<SubCategoryVM>();
    }
    public class SubCategoryVMDetails
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
