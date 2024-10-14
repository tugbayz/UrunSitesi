using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PackProApp.Enums;
using System.Linq.Expressions;

namespace PackProApp.Services.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AccountService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<bool> AnyAsync(Expression<Func<IdentityUser, bool>> expression)
        {
            return await _userManager.Users.AnyAsync(expression);
        }

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, Roles role)
        {
            var result = await _userManager.CreateAsync(user, "Password.1");
            if (!result.Succeeded)
            {
                return result;
            }
            return await _userManager.AddToRoleAsync(user, role.ToString());
        }

        public async Task<IdentityResult> DeleteByAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı." });
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeRolesResult.Succeeded)
            {
                return removeRolesResult;
            }

            return await _userManager.DeleteAsync(user);

        }

        public async Task<IdentityResult> UpdateEmailAsync(string userId, string newEmail)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı." });
            }

            // E-posta adresinin benzersiz olduğunu kontrol et
            if (await _userManager.FindByEmailAsync(newEmail) != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Bu e-posta adresi zaten kullanılıyor." });
            }

            user.Email = newEmail;
            user.NormalizedEmail = newEmail.ToUpperInvariant();
            user.UserName = newEmail;
            user.NormalizedUserName = newEmail.ToUpperInvariant();

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // E-posta değişikliğini onayla
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
                result = await _userManager.ChangeEmailAsync(user, newEmail, token);
            }

            return result;

        }
    }
}
