using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PackProApp.AppContext;
using PackProApp.Entities;
using PackProApp.Enums;

namespace PackProApp.Seeds
{
    public static class AdminSeed
    {
        private const string adminEmail = "admin@packpro.com";
        private const string adminPassword = "Password.1";

        public static async Task SeedAsync(IConfiguration configuration)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextBuilder.UseSqlServer(configuration.GetConnectionString("AppConnectionString"));

            AppDbContext context = new AppDbContext(dbContextBuilder.Options);
            if (!context.Roles.Any(x => x.Name == "Admin"))
            {
                await AddRoles(context);
            }
            if (!context.Users.Any(user => user.Email == adminEmail))
            {
                await AddAdmin(context);
            }
        }

        private static async Task AddAdmin(AppDbContext context)
        {
            IdentityUser user = new IdentityUser()
            {
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpperInvariant(),
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpperInvariant(),
                EmailConfirmed = true
            };
            user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, adminPassword);
            await context.Users.AddAsync(user);

            var adminRoleID = context.Roles.FirstOrDefault(role => role.Name == Roles.Admin.ToString()).Id;
            await context.UserRoles.AddAsync(new IdentityUserRole<string>()
            {
                RoleId = adminRoleID,
                UserId = user.Id
            });


            await context.Admins.AddAsync(new Admin()
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = adminEmail,
                IdentityId = user.Id
            });

            await context.SaveChangesAsync();
        }



        private static async Task AddRoles(AppDbContext context)
        {
            string[] roles = Enum.GetNames(typeof(Roles));

            for (int i = 0; i < roles.Length; i++)
            {
                if (await context.Roles.AnyAsync(role => role.Name == roles[i]))
                {
                    continue;
                }
                IdentityRole role = new IdentityRole()
                {
                    Name = roles[i],
                    NormalizedName = roles[i].ToUpperInvariant()
                };

                await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();
            }
        }
    }
}
