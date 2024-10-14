public class SubCategoryCreateDTO
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }  // Alt kategori bir ana kategoriye ait olmalı
    public Guid? ParentCategoryId { get; set; } // Üst kategori varsa, opsiyonel
}
