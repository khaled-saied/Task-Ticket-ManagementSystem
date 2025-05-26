using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticket_ManagementSystem.ViewModels;

namespace Ticket_ManagementSystem.Controllers
{
    public class AccountController(UserManager<ApplicationUser> _userManager,
                                    SignInManager<ApplicationUser> _signInManager) : Controller
    {

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This email is already registered.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Project");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(model);
                }
            }

            return View(model);
        }

        #endregion

        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is { })
            {
                var flag = await _userManager.CheckPasswordAsync(user, model.Password);

                if (flag)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);


                    if (result.IsNotAllowed)
                        ModelState.AddModelError(string.Empty, "You are not allowed to login.");

                    if (result.IsLockedOut)
                        ModelState.AddModelError(string.Empty, "Your account is locked out.");

                    if (result.Succeeded)
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            else
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        #endregion

    }
}
