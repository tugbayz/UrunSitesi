using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PackProApp.Areas.Admin.Modelss.ProductVMs;
using PackProApp.Areas.Customer.Models;
using PackProApp.Services.ImagesServices;
using PackProApp.Services.ProductServices;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PackProApp.Areas.Customer.Controllers
{
    public class HomeController : CustomerBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IProductService _productService;
        private readonly IImageService _imageService;
        public HomeController(UserManager<IdentityUser> userManager, IProductService productService, IImageService imageService)
        {
            _userManager = userManager;
            _productService = productService;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {

            var products = await _productService.GetAllAsync();

            foreach (var product in products.Data)
            {
                if (product.Image != null)
                {
                    product.ImagePath = await _imageService.GetImagePath(product.Id);  // ImagePath özelliğini dolduruyoruz
                }
            }

            return View(products.Data.Adapt<List<AdminProductListVM>>());




            // Oturum açan kullanıcının ID'sini alıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return NotFound(); // Eğer oturum açan kullanıcı yoksa NotFound döndür
            }

            // UserManager'dan kullanıcının bilgilerini alıyoruz
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(); // Eğer kullanıcı bulunamazsa hata döndür
            }

            // Kullanıcı bilgilerini ViewModel'e atıyoruz
            var customer = new CustomerVM
            {
                FirstName = user.UserName,  // IdentityUser'da FirstName yok, bu nedenle UserName kullanıyoruz
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };



            return View(customer); // View'a model gönderiyoruz
        }
    }
}
