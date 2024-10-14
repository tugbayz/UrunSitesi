using System.ComponentModel;

namespace PackProApp.Areas.Admin.Modelss.ProductVMs
{
    public class AdminProductDetailVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        [DisplayName("Category Name")]
        public string CategoryName { get; set; }
        public byte[]? Image { get; set; }
    }
}
