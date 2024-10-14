namespace PackProApp.DTOs.ProductDTOs
{
    public class ProductListDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; } // Ürünün ait olduğu kategori adı
        public byte[]? Image { get; set; }
        public string ImagePath { get; set; }
    }
}
