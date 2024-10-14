using PackProApp.Core.BaseEntities;

namespace PackProApp.Entities
{
    public class Admin : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IdentityId { get; set; }
        public byte[]? AdminImage { get; set; }
    }
}
