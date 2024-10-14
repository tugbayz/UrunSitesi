namespace PackProApp.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; } // Ürünün ait olduğu alt kategorinin ID'si
        public byte[]? Image { get; set; }
    }
}
