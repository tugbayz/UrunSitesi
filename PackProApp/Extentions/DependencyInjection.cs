using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using PackProApp.AppContext;
using PackProApp.Repositories.CategoryRepositories;
using PackProApp.Repositories.CustomerRepositories;
using PackProApp.Repositories.ProductRepositories;
using PackProApp.Seeds;
using PackProApp.Services.AccountServices;
using PackProApp.Services.CategoryServices;
using PackProApp.Services.CustomerServices;
using PackProApp.Services.ImagesServices;
using PackProApp.Services.MailServices;
using PackProApp.Services.ProductServices;
using System.Configuration;
using System.Globalization;
using System.Reflection;

namespace PackProApp.Extentions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(configuration.GetConnectionString("AppConnectionString"));
            });

            // Identity servislerini burada ekleyin
            services.AddIdentity<IdentityUser, IdentityRole>()
                   .AddEntityFrameworkStores<AppDbContext>()
                   .AddDefaultTokenProviders();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

           AdminSeed.SeedAsync(configuration).GetAwaiter().GetResult();

            return services;
        }

        public static IServiceCollection AddBusinessService(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageService, ImageService>();

            return services;
        }

        public static IServiceCollection AddMVCServices(this IServiceCollection services)
        {
            //referans type olan verilerin null olabilme ozelligini true yapar.
            services
                .AddControllersWithViews(opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                .AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix,
                opt => opt.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName!);
                    return factory.Create(nameof(SharedResource), assemblyName.Name!);
                });

            //Resource dosyası içinde SharedResource dosyalarını ara, classı degil.
            //SharedResource classını kendı olusturdugumuz resource dosyası için köpru olarak kullandık.
            services.AddLocalization(opt => opt.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(opt =>
            {
                var supportedCulture = new List<CultureInfo>()
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("tr-TR"),
                };
                opt.DefaultRequestCulture = new RequestCulture("en-US");
                opt.SupportedCultures = supportedCulture;   //arkadan gelen mesajlar için
                opt.SupportedUICultures = supportedCulture; //UI dan gelen elementler için
            });



            //LanguageViewLocation istenilen kaynaktan beslensin demek
            // ! koyunca hiç bir zaman null dönmeyecegine söz vermiş oluyoruz.



            return services;
        }
    }
}
