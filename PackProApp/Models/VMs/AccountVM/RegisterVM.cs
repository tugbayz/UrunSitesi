using PackProApp.Enums;

namespace PackProApp.Models.VMs.AccountVM
{
    public class RegisterVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public CustomerType CustomerType { get; set; } // Müşteri türü için alan
        public string? CompanyName { get; set; } // Eğer Kurumsal ise doldurulacak
        public string? VATNumber { get; set; } // Eğer Kurumsal ise doldurulacak
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }
    //    public string Email { get; set; }
    //    public string Password { get; set; }
    }
}
