using PackProApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace PackProApp.Areas.Admin.Modelss.CustomerVMs
{
    public class AdminCustomerDetailVM
    {
        [Display(Name = "Name")]
        public string FirstName { get; set; }

        [Display(Name = "Surname")]
        public string LastName { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Customer Type")]
        public CustomerType CustomerType { get; set; }

        [Display(Name = "Company Name")]
        public string? CompanyName { get; set; }

        [Display(Name = "VAT Number")]
        public string? VATNumber { get; set; }
    }
}

