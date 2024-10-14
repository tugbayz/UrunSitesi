using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using PackProApp.AppContext;
using PackProApp.Entities;
using PackProApp.Enums;
using PackProApp.Models.VMs.AccountVM;
using PackProApp.Services.MailServices;

namespace PackProApp.Controllers
{
    public class AccountController : BaseController
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly AppDbContext _context;


        private const string userPassword = "newPassword.1";

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IMailService mailService, AppDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mailService = mailService;
            _context = context;
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Kullanıcıya "Customer" rolü atanıyor
                await _userManager.AddToRoleAsync(user, Roles.Customer.ToString());

                // Müşteri bilgilerini kaydetme işlemleri
                var customer = new Customer
                {
                    IdentityId = user.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = "", // İsteğe bağlı, telefon numarası ekleyebilirsiniz
                    CustomerType = model.CustomerType,
                    CompanyName = model.CompanyName,
                    VATNumber = model.VATNumber,
                };
                _context.Customers.Add(customer);

                // Değişiklikleri veritabanına kaydet
                await _context.SaveChangesAsync();

                // Eğer CustomerService ile kaydetmek isterseniz burada kullanabilirsiniz
                // await _customerService.AddAsync(customer);

                // Müşteriye kayıt bilgilerini içeren e-posta gönderme
                var mailMessage = $"Merhaba {model.FirstName} {model.LastName},<br/><br/>" +
                                  $"Sisteme başarılı bir şekilde kaydoldunuz.<br/><br/>" +
                                  $"Kullanıcı Adınız: {model.Email}<br/>" +
                                  $"Şifreniz: {model.Password}<br/><br/>" +
                                  $"Giriş yapmak için <a href='{Url.Action("Login", "Account", null, Request.Scheme)}'>buraya tıklayın</a>.";
                await _mailService.SendMailAsync(model.Email, "Kayıt Bilgileriniz", mailMessage);

                return RedirectToAction("Index", "Home");
                await _userManager.AddToRoleAsync(user, Roles.Customer.ToString());
            }

            // Eğer kayıt başarısız olursa, hata mesajlarını göster
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterVM model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded)
        //    {
        //        // Müşteri bilgilerini kaydetme
        //        var customer = new Customer
        //        {
        //            FirstName = model.FirstName,
        //            LastName = model.LastName,
        //            Email = model.Email,
        //            PhoneNumber = "", 
        //            CustomerType = model.CustomerType,
        //            CompanyName = model.CompanyName,
        //            VATNumber = model.VATNumber,
        //        };

        //        // CustomerService üzerinden kaydedebilirsiniz
        //        // await _customerService.AddAsync(customer);

        //        // Müşteriye kayıt bilgilerini içeren e-posta gönderme
        //        var mailMessage = $"Merhaba {model.FirstName} {model.LastName},<br/><br/>" +
        //                          $"Sisteme başarılı bir şekilde kaydoldunuz.<br/><br/>" +
        //                          $"Kullanıcı Adınız: {model.Email}<br/>" +
        //                          $"Şifreniz: {model.Password}<br/><br/>" +
        //                          $"Giriş yapmak için <a href='{Url.Action("Login", "Account", null, Request.Scheme)}'>buraya tıklayın</a>.";
        //        await _mailService.SendMailAsync(model.Email, "Kayıt Bilgileriniz", mailMessage);

        //        return RedirectToAction("Index", "Home");
        //    }

        //    // Eğer kayıt başarısız olursa, hata mesajlarını göster
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError(string.Empty, error.Description);
        //    }

        //    return View(model);
        //}




        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterVM model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded)
        //    {
        //        var codeToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //        var callBackURL = Url.Action("ConfirmEmail", "Home", new { userId = user.Id, code = codeToken }, protocol: Request.Scheme);
        //        var mailMessage = $"Please confirm your account by <br/> <a href='{callBackURL}'> clicking here</a>";
        //        await _mailService.SendMailAsync(model.Email, "Confirm your mail", mailMessage);
        //        return RedirectToAction("Index");
        //    }
        //    return RedirectToAction("Index");
        //}

        //public async Task<IActionResult> ConfirmEmail(string userId, string code)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    await _userManager.ConfirmEmailAsync(user, code);
        //    return RedirectToAction("Login");
        //}

        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            var checkPassword = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!checkPassword.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
                return View(model);
            }

            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole == null || userRole.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcıya atanan bir rol bulunamadı.");
                return View(model);
            }

           


            return RedirectToAction("Index", "Home", new { Area = userRole[0] });
        }





        //[HttpPost]
        //public async Task<IActionResult> Login(LoginVM model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        await Console.Out.WriteLineAsync("Kullanıcı adı veya şifre hatalı");
        //        return View(model);
        //    }
        //    var checkPassword = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        //    if (!checkPassword.Succeeded)
        //    {
        //        await Console.Out.WriteLineAsync("Kullanıcı adı veya şifre hatalı");
        //        return View(model);
        //    }

        //    var userRole = await _userManager.GetRolesAsync(user);
        //    if (userRole == null)
        //    {
        //        await Console.Out.WriteLineAsync("Kullanıcı adı veya şifre hatalı");
        //        return View(model);
        //    }

        //    return RedirectToAction("Index", "Home", new { Area = userRole[0].ToString() });
        //}

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");

        }
    }
}
