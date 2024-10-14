using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackProApp.Core.BaseEntities;

namespace PackProApp.Core.BaseEntityConfigurations
{
    public class AudiTableEntityConfiguration<TEntity> : BaseEntityConfiguration<TEntity> where TEntity : AuditableEntity
    {
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.DeletedBy).IsRequired(false);
            builder.Property(x => x.DeletedDate).IsRequired(false);

            base.Configure(builder);
        }
    }
}
