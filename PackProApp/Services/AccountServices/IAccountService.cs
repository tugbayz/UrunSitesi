using Microsoft.AspNetCore.Identity;
using PackProApp.Enums;
using System.Linq.Expressions;

namespace PackProApp.Services.AccountServices
{
    public interface IAccountService
    {
        Task<bool> AnyAsync(Expression<Func<IdentityUser, bool>> expression);

        Task<IdentityResult> CreateUserAsync(IdentityUser user, Roles role);

        Task<IdentityResult> DeleteByAsync(string userId);

        Task<IdentityResult> UpdateEmailAsync(string userId, string newEmail);
    }
}
