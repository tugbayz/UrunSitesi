namespace PackProApp.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; } // Ürünün ait olduğu kategori ID'si
        public string CategoryName { get; set; } // Kategori adı
        public byte[]? Image { get; set; }
    }
}
