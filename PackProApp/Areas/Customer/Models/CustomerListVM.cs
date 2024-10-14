using PackProApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace PackProApp.Areas.Customer.Models
{
    public class CustomerListVM
    {
        public Guid Id { get; set; }
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Müşteri Tipi")]
        public CustomerType CustomerType { get; set; }

        [Display(Name = "Şirket Adı")]
        public string? CompanyName { get; set; }

        [Display(Name = "Vergi Numarası")]
        public string? VATNumber { get; set; }
    }
}
