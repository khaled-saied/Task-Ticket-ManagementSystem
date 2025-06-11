using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ticket_ManagementSystem.Helper;
using Ticket_ManagementSystem.Utilities;
using Ticket_ManagementSystem.ViewModels.AccountViewModel;

namespace Ticket_ManagementSystem.Controllers
{
    public class AccountController(UserManager<ApplicationUser> _userManager,
                                    SignInManager<ApplicationUser> _signInManager,
                                    IMailService _mailService) : Controller
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

        //Login With Google
        public IActionResult LoginWithGoolge()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value,
            });
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Signout
        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion

        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Email not found.");
                    return View(model);
                }
                else
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

                    // Here you would typically send the reset link via email.
                    var email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = $"Please reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>"
                    };
                    // Send email logic goes here, e.g., using an email service.
                    #region Old Way
                    //var flag = EmailSettings.SendEmail(email);
                    //if (flag)
                    //{
                    //    //Check Your Inbox
                    //    return RedirectToAction("CheckYourInbox");
                    //}
                    #endregion

                    _mailService.Send(email);
                    return RedirectToAction("CheckYourInbox");

                }
            }
            ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
            return View(model);
        }

        //Check Your Inbox
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        //Reset Password
        #region Reset Password
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
            {
                return BadRequest("Invalid token or email.");
            }
           
            TempData["Token"] = token;
            TempData["Email"] = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var token = TempData["Token"] as string;
                var email = TempData["Email"] as string;
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError(string.Empty, "Invalid token or email.");
                    return View(model);
                }
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                    return View(model);
                }
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        #endregion


        #endregion
    }
}
