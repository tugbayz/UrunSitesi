namespace PackProApp.DTOs.ProductDTOs
{
    public class ProductUpdateDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; } // Ürünün güncellenirken ait olduğu alt kategori ID'si
    }
}
