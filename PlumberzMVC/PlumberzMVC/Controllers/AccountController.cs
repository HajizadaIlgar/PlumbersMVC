using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlumberzMVC.Extensions;
using PlumberzMVC.Models;
using PlumberzMVC.ViewModels;

namespace PlumberzMVC.Controllers
{
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager) : Controller
    {
        public bool IsAuthenticated => HttpContext.User.Identity?.IsAuthenticated ?? false;
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser
            {
                FullName = vm.FullName,
                Email = vm.Email,
                UserName = vm.UserName,
            };
            var usercreate = await _userManager.CreateAsync(user, vm.Password);

            if (!usercreate.Succeeded)
            {
                foreach (var err in usercreate.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View(vm);
            }
            var roleresult = await _userManager.AddToRoleAsync(user, Roles.User.GetRole());
            if (!roleresult.Succeeded)
            {
                foreach (var err in roleresult.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
                return View(vm);
            }

            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm, string? returUrl = null)
        {
            if (IsAuthenticated) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View();
            AppUser user = null;
            if (vm.UserNameorEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(vm.UserNameorEmail);
            }
            else
            {
                user = await _userManager.FindByEmailAsync(vm.UserNameorEmail);
            }


            //if (string.IsNullOrWhiteSpace(returUrl))
            //{
            //    if (await _userManager.IsInRoleAsync(user, "Admin"))
            //    {
            //        return RedirectToAction("Index", new { Controller = "Dashboard", Area = "Admin" });
            //    }
            //    return RedirectToAction("Index", "Home");
            //}

            if (user is null)
            {
                ModelState.AddModelError("", "Bele bir istifadeci tapilmadi !!!");
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Sen 1 deqiqelik bloklandin gede");
                }
                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "username ve ya password sehvdi");
                }
                return View(vm);
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> MyRoleMethod()
        {
            // Admin istifadəçisini tapmaq
            var data = await _userManager.FindByNameAsync("admin");

            if (data == null)
            {
                AppUser adminUser = new AppUser
                {
                    FullName = "Admin",
                    UserName = "admin",
                    Email = "admin@mail.com",
                };

                // Admin istifadəçisini yaratmaq
                var adminCreateResult = await _userManager.CreateAsync(adminUser, "123456");

                if (!adminCreateResult.Succeeded)
                {
                    return BadRequest("Admin istifadəçisini yaratmaq mümkün olmadı.");
                }

                // Admin rolunu yoxlamaq və əlavə etmək
                if (!await _roleManager.RoleExistsAsync(Roles.Admin.GetRole()))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.GetRole()));

                    if (!roleResult.Succeeded)
                    {
                        return BadRequest("Admin rolu yaradılmadı.");
                    }
                }

                // İstifadəçini rola əlavə etmək
                var roleResultt = await _userManager.AddToRoleAsync(adminUser, Roles.Admin.GetRole());
                if (!roleResultt.Succeeded)
                {
                    return BadRequest("Admin roluna əlavə edilmədi.");
                }
            }

            // Digər rolları yoxlamaq və yaradmaq
            foreach (Roles item in Enum.GetValues(typeof(Roles)))
            {
                var roleName = item.GetRole();
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!createRoleResult.Succeeded)
                    {
                        return BadRequest($"Rol '{roleName}' yaradılmadı.");
                    }
                }
            }

            // Hər şey uğurla başa çatdı
            return RedirectToAction("Login", "Account");
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
