using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlumberzMVC.Extensions;
using PlumberzMVC.Models;
using PlumberzMVC.ViewModels;
using PlumberzMVC.Views.Account.Enums;

namespace PlumberzMVC.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager, SignInManager<AppUser> _signInManager) : Controller
    {
        private bool IsAuthenticated => HttpContext.User.Identity?.IsAuthenticated ?? false;
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appuser = new AppUser
            {
                FullName = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username,
            };
            var result = await _userManager.CreateAsync(appuser, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            var roleResult = await _userManager.AddToRoleAsync(appuser, Roles.User.GetRole());
            if (!roleResult.Succeeded)
            {
                foreach (var err in roleResult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View();
            }
            return View();
        }
        public async Task<IActionResult> MyRolesMethod()
        {
            foreach (Roles item in Enum.GetValues(typeof(Roles)))
            {
                await _roleManager.CreateAsync(new IdentityRole(item.GetRole()));
            }
            return Ok();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl = null)
        {
            if (IsAuthenticated) RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View();
            AppUser? user = null;
            if (vm.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            }
            if (user is null)
            {
                ModelState.AddModelError("", "Username Or Password is wrong!!!");
                return View();
            }
            var password = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);
            if (!password.Succeeded)
            {
                if (password.IsLockedOut)
                {
                    ModelState.AddModelError("", "Wait untill" + user.LockoutEnd.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                if (password.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
            }
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Index", new { Controller = "Dashbard", Area = "Admin" });
                }
                return RedirectToAction("Index", "Home");
            }
            return LocalRedirect(returnUrl);
        }
    }
}
