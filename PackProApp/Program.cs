using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using PackProApp.AppContext;
using PackProApp.Extentions;

namespace PackProApp

{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddInfrastructureService(builder.Configuration); // Identity burada eklenecek
            builder.Services.AddBusinessService();
            builder.Services.AddMVCServices();
         

            // Add AspNetCoreHero.ToastNotification services
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopRight;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseRequestLocalization();
            app.UseAuthentication(); // UseAuthentication middleware should be here
            app.UseAuthorization();

            // Add Notyf middleware
            app.UseNotyf();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapDefaultControllerRoute();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapAreaControllerRoute(
            //        name: "Admin",
            //        areaName: "Admin",
            //        pattern:"Admin/{controller=Home}/{action=Index}/{id?}"
            //       );
            //    endpoints.MapDefaultControllerRoute(); 
            //});

            app.Run();
        }
    }
}