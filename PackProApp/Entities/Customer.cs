using PackProApp.Core.BaseEntities;
using PackProApp.Enums;

namespace PackProApp.Entities
{
    public class Customer : AuditableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public CustomerType CustomerType { get; set; }
        public string? CompanyName { get; set; }
        public string? VATNumber { get; set; }
        public string IdentityId { get; set; }

    }
}
