using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackProApp.Core.BaseEntityConfigurations;
using PackProApp.Entities;

namespace PackProApp.Configurations
{
    public class CustomerConfiguration: AudiTableEntityConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15);

            // Kurumsal müşteri ise şirket ismi ve vergi numarası zorunlu olabilir
            builder.Property(c => c.CompanyName)
                .HasMaxLength(100);

            builder.Property(c => c.VATNumber)
                .HasMaxLength(15);

            
        }
    }
}
