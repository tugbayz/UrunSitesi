using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackProApp.Core.BaseEntityConfigurations;
using PackProApp.Entities;

namespace PackProApp.Configurations
{
    public class AdminConfigurations : AudiTableEntityConfiguration<Admin>
    {
        public override void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.Property(a => a.FirstName).IsRequired().HasMaxLength(120);
            builder.Property(a => a.LastName).IsRequired().HasMaxLength(120);
            builder.Property(a => a.Email).IsRequired().HasMaxLength(120);
            builder.Property(a => a.IdentityId).IsRequired();
            base.Configure(builder);
        }
    }
}
