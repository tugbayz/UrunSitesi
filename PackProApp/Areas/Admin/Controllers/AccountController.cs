using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PackProApp.AppContext;
using PackProApp.Areas.Admin.Modelss;

namespace PackProApp.Areas.Admin.Controllers
{
    public class AccountController : AdminBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Şu anki giriş yapan kullanıcıyı alıyoruz
            return View();
        }

        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" }); // Giriş yapmamışsa giriş sayfasına yönlendirme
            }
            var trimmedUsername = user.UserName.Length > 5 ? user.UserName.Substring(0, 5) : user.UserName;
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.IdentityId == user.Id);
            if (admin == null)
            {
                return RedirectToAction("Error");
            }
            // Kullanıcı bilgilerini ProfileViewModel'e taşıyoruz
            var profileVm = new AdminProfilVM
            {
                Username = trimmedUsername,
                Email = user.Email,
                AdminImage = admin.AdminImage
                // Diğer gerekli bilgileri buraya ekleyebilirsiniz
            };

            return View(profileVm);
        }
        // GET: Update
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            // Kullanıcıya ait admin bilgilerini alıyoruz
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.IdentityId == user.Id);
            if (admin == null)
            {
                return RedirectToAction("Error");
            }

            // Mevcut profil bilgilerini ProfileViewModel'e taşıyoruz
            var profileVm = new AdminProfilVM
            {
                Username = user.UserName,
                Email = user.Email,
                AdminImage = admin.AdminImage
            };

            // Görünümü model ile birlikte döndür
            return View(profileVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AdminProfilVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.IdentityId == user.Id);
            if (admin == null)
            {
                return RedirectToAction("Error");
            }

            // Yeni resim yüklenmişse veritabanında güncelle
            if (model.NewImage != null && model.NewImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await model.NewImage.CopyToAsync(memoryStream);
                    admin.AdminImage = memoryStream.ToArray();
                }
            }

            admin.FirstName = model.Username;
            admin.Email = model.Email;

            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();

            SuccesNotyf("Profile updated successfully");
            return RedirectToAction("Profile");
        }

    }
}
