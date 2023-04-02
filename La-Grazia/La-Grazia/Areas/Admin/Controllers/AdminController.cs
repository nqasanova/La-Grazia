using System;
using La_Grazia.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using La_Grazia.Database.Models;

namespace La_Grazia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]

    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Index

        public IActionResult Index()
        {
            List<User> admins = _userManager.Users.ToList();

            return View(admins);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create(AdminViewModel adminVM)
        {
            if (!ModelState.IsValid) return View();

            User admin = await _userManager.FindByNameAsync(adminVM.UserName);
            if (admin != null)
            {
                ModelState.AddModelError("UserName", "Username is already taken!");
                return View();
            }

            admin = await _userManager.FindByEmailAsync(adminVM.Email);
            if (admin != null)
            {
                ModelState.AddModelError("Email", "Email is already taken!");
                return View();
            }

            admin = new User
            {
                Email = adminVM.Email,
            };

            var result = await _userManager.CreateAsync(admin, adminVM.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            await _userManager.AddToRoleAsync(admin, "Admin");
            await _signInManager.SignInAsync(admin, true);

            return RedirectToAction("index", "dashboard");
        }

        #endregion

        #region Delete

        public async Task<IActionResult> Delete(string id)
        {
            User deleteAdmin = _userManager.Users.FirstOrDefault();

            await _userManager.DeleteAsync(deleteAdmin);
            await _userManager.UpdateAsync(deleteAdmin);

            return RedirectToAction("index");
        }

        #endregion
    }
}