using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using PackProApp.Areas.Customer.Models;
using PackProApp.DTOs.CustomerDTOs;
using PackProApp.Services.CustomerServices;
using PackProApp.Services.ProductServices;
using PagedList;
using System.Security.Claims;

namespace PackProApp.Areas.Customer.Controllers
{
    public class CustomerController : CustomerBaseController
    {
        private readonly ICustomerService _customerService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStringLocalizer<SharedResource> _stringLocalizer;
       

        public CustomerController(ICustomerService customerService, UserManager<IdentityUser> userManager, IStringLocalizer<SharedResource> stringLocalizer)
        {
            _customerService = customerService;
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
            
        }




        public async Task<IActionResult> Index()
        {
            return View();
        }

        // Müşteri kendi profilini düzenleyebilir (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // ID'yi kullanıcıdan alabilir veya User.Identity'den getirebiliriz
            var result = await _customerService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                var message = _stringLocalizer["Customer not found"];
                ErrorNotyf(message);
                return RedirectToAction("Index", "Home");
            }

            var model = result.Data.Adapt<CustomerUpdateVM>();
            return View(model);
        }

        // Müşteri kendi profilini düzenleyebilir (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(CustomerUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _customerService.UpdateByAsync(model.Adapt<CustomerUpdateDTO>());
                if (result.IsSuccess)
                {
                    var message = _stringLocalizer["An unknown error occurred."];
                    ErrorNotyf(message);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty,_stringLocalizer["Customer edited successfully"]);
            }
            return View(model);
        }

        public async Task<IActionResult> Profile()
        {
            // Giriş yapan kullanıcının ID'sini alıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcıyı UserManager ile alıyoruz
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcı bilgilerini View'a gönderiyoruz
            return View(user);
        }
    }
}
