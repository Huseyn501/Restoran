using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestoranMVC.DAL;
using RestoranMVC.Models;

namespace RestoranMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(opt => {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
                
            );

            builder.Services.AddIdentity<AppUser, IdentityRole>(apt =>
            {
                apt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRTUVWXYZ0123456789";
                apt.User.RequireUniqueEmail = false;
                apt.Password.RequireUppercase = false;
                apt.Password.RequireLowercase = false;
                apt.Password.RequireNonAlphanumeric = false;

            }).AddEntityFrameworkStores<AppDbContext>();

            var app = builder.Build();
            app.UseStaticFiles();
            app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
          );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}"
                );
            app.Run();
        }
    }
}
