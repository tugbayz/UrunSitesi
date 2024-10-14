using PackProApp.Core.Interfaces;

namespace PackProApp.Core.BaseEntities
{
    public class AuditableEntity : BaseEntity, IDeletableEntity
    {
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

    }
}
