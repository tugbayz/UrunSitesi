using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PackProApp.Areas.Admin.Modelss.AdminVM;
using PackProApp.Services.AccountServices;

namespace PackProApp.Areas.Admin.Controllers
{
    public class AdminController : AdminBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index()
        {

            // Şu anki kullanıcıyı al
            return View();
        }
    }
}
