using techXpress.Services.Abstraction;
using techXpress.Services.Managers;
using techXpress.Services.Services;
using techXpress.DataAccess.Abstraction;
using techXpress.DataAccess.Data;
using techXpress.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using techXpress.DataAccess.Entities;
using Stripe;
using Rotativa.AspNetCore;
using techXpress.UI.Models;

namespace techXpress.UI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
                optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IProductManager, ProductManager>();
            builder.Services.AddTransient<IFilesService, FilesService>(); 
            builder.Services.AddTransient<IOrderManger, OrderManger>(); 

            builder.Services.AddScoped<ICategoryManager, CategoryManager>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(7); 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.MaxAge = TimeSpan.FromDays(7); 
            });

            builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;

            }).AddEntityFrameworkStores<AppDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
            });


            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

            RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            await SeedApplicationDataAsync(app.Services, app.Configuration);
            await app.RunAsync();
        }

        private static async Task SeedApplicationDataAsync(
            IServiceProvider services,
            IConfiguration configuration)
        {
            using IServiceScope scope = services.CreateScope();
            RoleManager<IdentityRole<Guid>> roleManager = scope.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            string[] roles = [UserRole.Admin, UserRole.Seller, UserRole.Customer];
            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (!await dbContext.Sellers.AnyAsync())
            {
                dbContext.Sellers.Add(new Seller
                {
                    SellerName = "TechXpress",
                    StoreName = "TechXpress"
                });
                await dbContext.SaveChangesAsync();
            }

            string? adminEmail = configuration["SeedAdmin:Email"];
            string? adminPassword = configuration["SeedAdmin:Password"];
            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                User? admin = await userManager.FindByEmailAsync(adminEmail);
                if (admin == null)
                {
                    admin = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = adminEmail,
                        UserName = configuration["SeedAdmin:UserName"] ?? adminEmail
                    };

                    IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                    if (!result.Succeeded)
                    {
                        string errors = string.Join("; ", result.Errors.Select(error => error.Description));
                        throw new InvalidOperationException($"Could not create the initial admin: {errors}");
                    }
                }

                if (!await userManager.IsInRoleAsync(admin, UserRole.Admin))
                {
                    await userManager.AddToRoleAsync(admin, UserRole.Admin);
                }
            }
        }
    }
}
