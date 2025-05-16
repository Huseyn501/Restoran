using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RestoranMVC.Helpers;
using RestoranMVC.Models;
using RestoranMVC.ViewModels;
using System.Threading.Tasks;

namespace RestoranMVC.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> userManager;
        RoleManager<IdentityRole> RoleManager;
        SignInManager<AppUser> SignInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser()
            {
                Name = registerVm.Name,
                Surname = registerVm.Surname,
                Email = registerVm.Email,
                UserName = registerVm.Username,
            };
            var result = await userManager.CreateAsync(user, registerVm.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await userManager.FindByEmailAsync(loginVm.Email) ?? await userManager.FindByNameAsync(loginVm.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "user not found");
                return View();
            }
            var result = await SignInManager.CheckPasswordSignInAsync(appUser, loginVm.Password, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Wrong password");
                return View();
            }
            await SignInManager.SignInAsync(appUser, false);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                await RoleManager.CreateAsync(new IdentityRole()
                {
                    Name = item.ToString(),
                });
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
