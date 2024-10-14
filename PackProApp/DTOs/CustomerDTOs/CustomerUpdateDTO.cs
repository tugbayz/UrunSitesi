using PackProApp.Enums;

namespace PackProApp.DTOs.CustomerDTOs
{
    public class CustomerUpdateDTO
    {
        public Guid Id { get; set; } // Güncellenen müşteri için ID gereklidir
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public CustomerType CustomerType { get; set; }
        public string? CompanyName { get; set; }
        public string? VATNumber { get; set; }
    }
}
