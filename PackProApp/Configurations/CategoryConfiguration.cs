using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackProApp.Core.BaseEntityConfigurations;
using PackProApp.Entities;

namespace PackProApp.Configurations
{
    public class CategoryConfiguration : AudiTableEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Description)
                .HasMaxLength(250);

            // Kategori ve alt kategoriler arasındaki ilişki
            //builder.HasMany(c => c.SubCategories)
            //    .WithOne(sc => sc.ParentCategory)
            //    .HasForeignKey(sc => sc.ParentCategoryId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // Kategori ve ürünler arasındaki ilişki
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}
