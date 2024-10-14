using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackProApp.Core.BaseEntityConfigurations;
using PackProApp.Entities;

namespace PackProApp.Configurations
{
    public class ProductConfiguration : AudiTableEntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            // Name özelliği için zorunluluk ve maksimum uzunluk belirleniyor
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(128);

            // Price özelliği için zorunluluk belirleniyor
            builder.Property(p => p.Price)
                .IsRequired();

            // Category ile ilişki tanımlanıyor
            builder.HasOne(p => p.Category) // Ürün, bir kategoriye ait
                .WithMany(c => c.Products)   // Kategori, birden fazla ürüne sahip olabilir
                .HasForeignKey(p => p.CategoryId) // Yabancı anahtar olarak CategoryId kullanılıyor
                .OnDelete(DeleteBehavior.Cascade); // Kategori silindiğinde, ilişkili ürünler de silinir

            base.Configure(builder);
        }
    }
}
