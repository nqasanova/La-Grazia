using System;
using La_Grazia.ViewModels;
using La_Grazia.Database.Models;
using La_Grazia.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using La_Grazia.Database;
using Microsoft.EntityFrameworkCore;
using La_Grazia.Database.Models.Enums;
using Newtonsoft.Json;

namespace La_Grazia.Areas.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(DataContext datacontext, UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _datacontext = datacontext;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Register()
        {
            RegisterViewModel memberRegisterVM = new RegisterViewModel { };
            return PartialView("_RegisterModalPartial", memberRegisterVM);
        }

        [HttpPost, ActionName("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPost(RegisterViewModel registerVM)
        {
            TempData["Register"] = false;

            if (!ModelState.IsValid) return RedirectToAction("index", "home");

            if (registerVM == null)
            {
                return RedirectToAction("index", "home");
            }

            if (registerVM.UserName == null)
            {
                return RedirectToAction("index", "home");
            }

            if (registerVM.Email == null)
            {
                return RedirectToAction("index", "home");
            }

            if (registerVM.FullName == null)
            {
                return RedirectToAction("index", "home");
            }

            User member = await _userManager.FindByNameAsync(registerVM.UserName);

            if (member != null)
            {
                ModelState.AddModelError("UserName", "UserName already taken!");
                return RedirectToAction("index", "error");
            }

            member = await _userManager.FindByEmailAsync(registerVM.Email);

            if (member != null)
            {
                ModelState.AddModelError("Email", "Email already taken!");
                return RedirectToAction("index", "error");
            }

            member = new User
            {
                FullName = registerVM.FullName,
                UserName = registerVM.UserName,
                Email = registerVM.Email
            };

            if (registerVM.Password == null)
            {
                return RedirectToAction("index", "error");
            }

            var result = await _userManager.CreateAsync(member, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction("index", "error");
            }

            await _userManager.AddToRoleAsync(member, "Member");
            await _signInManager.SignInAsync(member, true);

            TempData["Register"] = true;

            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/templates/Register.html"))
            {
                body = reader.ReadToEnd();
            }

            _emailService.Send(registerVM.Email, "Welcome to Ulvino", body);

            return RedirectToAction("index", "home");
        }


        public IActionResult Login()
        {
            LoginViewModel memberLoginVM = new LoginViewModel { };
            return PartialView("_LoginModalPartial", memberLoginVM);
        }

        [HttpPost, ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPost(LoginViewModel loginVM)
        {
            TempData["Login"] = false;

            if (!ModelState.IsValid) return RedirectToAction("index", "home");

            if (loginVM == null)
            {
                return RedirectToAction("index", "home");
            }


            User member = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginVM.UserName);

            if (member == null)
            {
                ModelState.AddModelError("", "username or password incorrect!");
                return RedirectToAction("index", "home");
            }

            var result = await _signInManager.PasswordSignInAsync(member, loginVM.Password, true, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "username or password incorrect!");
                return RedirectToAction("index", "home");

            }

            User admin = null;

            admin = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginVM.UserName && x.IsAdmin);

            if (admin != null)
            {
                return RedirectToAction("index", "dashboard", new { area = "manage" });
            }

            TempData["Login"] = true;

            return RedirectToAction("index", "home");
        }


        public IActionResult ForgotPassword()
        {
            ForgotPasswordViewModel forgotPasswordVM = new ForgotPasswordViewModel { };
            return PartialView("_ForgotModalPartial", forgotPasswordVM);
        }

        [HttpPost, ActionName("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            User user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email is not valid!");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string callback = Url.Action("resetpassword", "account", new { token, email = user.Email }, Request.Scheme);

            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/templates/forgotpassword.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{{url}}", callback);

            _emailService.Send(user.Email, "Reset password", body);

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            ResetPasswordViewModel resetPasswordVM = new ResetPasswordViewModel
            {
                Token = token,
                Email = email
            };

            return View(resetPasswordVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
        {
            if (!ModelState.IsValid) return View(resetPasswordVM);

            User user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Token");
                return View(resetPasswordVM);
            }

            var resetResult = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.Password);

            if (!resetResult.Succeeded)
            {
                foreach (var item in resetResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View(resetPasswordVM);
            }

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            User member = await _userManager.FindByNameAsync(User.Identity.Name);

            ProfileViewModel profileVM = new ProfileViewModel
            {
                Email = member.Email,
                FullName = member.FullName,
                PhoneNumber = member.PhoneNumber,
                UserName = member.UserName,
                Orders = _datacontext.Orders.Include(x => x.OrderedProducts).Where(x => x.UserId == member.Id).ToList()
            };

            return View(profileVM);
        }

        [HttpPost]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel profileVM)
        {
            TempData["ProfileEdit"] = false;

            if (!ModelState.IsValid) return RedirectToAction("profile");

            User member = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!string.IsNullOrWhiteSpace(profileVM.ConfirmNewPassword) && !string.IsNullOrWhiteSpace(profileVM.NewPassword))
            {
                var passwordChangeResult = await _userManager.ChangePasswordAsync(member, profileVM.CurrentPassword, profileVM.NewPassword);

                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var item in passwordChangeResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return RedirectToAction("profile");
                }
            }

            if (member.Email != profileVM.Email && _userManager.Users.Any(x => x.NormalizedEmail == profileVM.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "This email has already been taken!");
                return RedirectToAction("profile");
            }

            member.FullName = profileVM.FullName;
            member.Email = profileVM.Email;
            member.PhoneNumber = profileVM.PhoneNumber;

            var result = await _userManager.UpdateAsync(member);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return RedirectToAction("profile");
            }

            TempData["ProfileEdit"] = true;

            return RedirectToAction("profile");
        }
    }
}