using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using PackProApp.Areas.Admin.Modelss.AdminVM;
using PackProApp.Areas.Admin.Modelss.ProductVMs;
using PackProApp.Extentions;
using PackProApp.Services.AccountServices;
using PackProApp.Services.ImagesServices;
using PackProApp.Services.ProductServices;

namespace PackProApp.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IProductService _productService;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
      
        private readonly IImageService _imageService;


        public HomeController(IAccountService accountService, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IProductService productService, IStringLocalizer<SharedResource> stringLocalizer, IImageService imageService)
        {
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
            _productService = productService;
            _stringLocalizer = stringLocalizer;
            _imageService = imageService;
        }

        //public async Task<IActionResult> Index()
        //{
        //    ViewBag.Message = _stringLocalizer["Welcome"];
        //    ViewBag.Message = _htmlLocalizer["Welcome"];


        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return NotFound("Kullanıcı bulunamadı.");
        //    }

        //    // Tüm ürünleri veritabanından al
        //    var products = await _productService.GetAllProductsAsync();

        //    // Kullanıcı bilgilerini ve tüm ürünleri ViewModel'e aktar
        //    var viewModel = new AdminListVM
        //    {
        //        Id = user.Id,
        //        Username = user.UserName,
        //        Email = user.Email,
        //        Products = products // Tüm ürünleri ekle
        //    };

        //    return View(viewModel);
        //}

        public async Task<IActionResult> Index(int? page)
        {
           

            var pageNumber = page ?? 1;
            var pageSize = 3;

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                var message2 = _stringLocalizer["User not found."];
                return NotFound(message2);
            }

            var products = await _productService.GetAllProductsAsync();

            if (products == null || !products.Any())
            {
                var message1 = _stringLocalizer["No products found to list"];
                ErrorNotyf(message1);
                var emptyViewModel = new AdminListVM
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Products = new List<AdminProductListVM>()
                };
                return View(emptyViewModel);
            }

            var viewModel = new AdminListVM
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Products = products.Adapt<List<AdminProductListVM>>()
            };

            foreach (var item in viewModel.Products)
            {
                if (item.Image != null)
                {
                    item.ImageBase64 = item.Image.ToBase64String();
                }
            }

            return View(viewModel);
        }

      






    }
}
