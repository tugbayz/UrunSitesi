using PackProApp.Areas.Admin.Modelss.ProductVMs;
using PackProApp.DTOs.ProductDTOs;
using System.ComponentModel.DataAnnotations;

namespace PackProApp.Areas.Admin.Modelss.AdminVM
{
    public class AdminListVM
    {
        public string Id { get; set; } // Kullanıcı kimliği
        [Required]
        public string Username { get; set; } // Kullanıcı adı
        public byte[]? Image { get; set; }
        public byte[]? AdminImage { get; set; } // Profilde gösterilecek resim

        public IEnumerable<AdminProductListVM> Products { get; set; } // Ürünleri liste olarak ekleyin

        [Required, EmailAddress]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        //public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
